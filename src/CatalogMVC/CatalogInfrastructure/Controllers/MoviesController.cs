using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CatalogDomain.Model;
using CatalogInfrastructure;
using CatalogInfrastructure.Services;
using NuGet.Protocol.Plugins;

namespace CatalogInfrastructure.Controllers
{
    public class MoviesController : Controller
    {
        private readonly DbCatalogContext _context;
        
        private readonly IReportService _reportService;

        private readonly MovieDataPortServiceFactory _movieDataPortServiceFactory;

        public MoviesController(DbCatalogContext context, MovieDataPortServiceFactory movieDataPortServiceFactory, IReportService reportService)
        {
            _context = context;
            _movieDataPortServiceFactory = movieDataPortServiceFactory;
            _reportService = reportService;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Movies.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
           
            if (id == null) return NotFound();

            var movie = await _context.Movies
                .Include(m => m.MovieGenres)
                    .ThenInclude(mg => mg.Genre)
                .Include(m => m.MovieCasts)
                    .ThenInclude(mc => mc.Actor)
                .Include(m => m.DirectedBies)
                    .ThenInclude(mc => mc.Director)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (movie == null) return NotFound();

            return View(movie);
        }

        public IActionResult Create()
        {
            ViewBag.Genres = new MultiSelectList(_context.Genres.ToList(), "Id", "GenreName");
            ViewBag.Actors = new MultiSelectList(_context.Actors.ToList(), "Id", "FullName");
            ViewBag.Directors = new MultiSelectList(_context.Directors.ToList(), "Id", "FullName");

            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Movie movie, List<int> selectedGenres, List<int> selectedActors, List<int> selectedDirectors)
        {
            if (ModelState.IsValid)
            {
                _context.Movies.Add(movie);
                await _context.SaveChangesAsync();

                if(selectedGenres != null)
                {
                    foreach (var genreId in selectedGenres)
                    {
                        if (!_context.MovieGenres.Any(mg => mg.MovieId == movie.Id && mg.GenreId == genreId))
                        {
                            _context.MovieGenres.Add(new MovieGenre { MovieId = movie.Id, GenreId = genreId });
                        }
                    }
                    await _context.SaveChangesAsync();
                }

                if (selectedActors != null)
                {
                    foreach (var actorId in selectedActors)
                    {
                        if (!_context.MovieCasts.Any(mg => mg.MovieId == movie.Id && mg.ActorId == actorId))
                        {
                            _context.MovieCasts.Add(new MovieCast { MovieId = movie.Id, ActorId = actorId });
                        }
                    }
                    await _context.SaveChangesAsync();
                }

                if (selectedDirectors != null)
                {
                    foreach (var directorId in selectedDirectors)
                    {
                        if (!_context.DirectedBies.Any(mg => mg.MovieId == movie.Id && mg.DirectorId == directorId))
                        {
                            _context.DirectedBies.Add(new DirectedBy { MovieId = movie.Id, DirectorId = directorId });
                        }
                    }
                    await _context.SaveChangesAsync();
                }

                return RedirectToAction(nameof(Index));
            }

            ViewBag.Genres = new MultiSelectList(await _context.Genres.ToListAsync(), "Id", "GenreName", selectedGenres);
            ViewBag.Actors = new MultiSelectList(await _context.Actors.ToListAsync(), "Id", "FullName", selectedActors);
            ViewBag.Directors = new MultiSelectList(await _context.Directors.ToListAsync(), "Id", "FullName", selectedDirectors);

            return View(movie);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var movie = await _context.Movies
                .Include(m => m.MovieGenres)
                    .ThenInclude(mg => mg.Genre)
                .Include(m => m.MovieCasts)
                    .ThenInclude(mc => mc.Actor)
                .Include(m => m.DirectedBies)
                    .ThenInclude(mc => mc.Director)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (movie == null) return NotFound();

            var selectedGenres = movie.MovieGenres.Select(mg => mg.GenreId).ToList();
            var selectedActors = movie.MovieCasts.Select(mg => mg.ActorId).ToList();
            var selectedDirectors = movie.DirectedBies.Select(mg => mg.DirectorId).ToList();

            ViewData["Genres"] = new SelectList(await _context.Genres.ToListAsync(), "Id", "GenreName");
            ViewData["Actors"] = new SelectList(await _context.Actors.ToListAsync(), "Id", "FullName");
            ViewData["Directors"] = new SelectList(await _context.Directors.ToListAsync(), "Id", "FullName");

            ViewBag.SelectedGenres = selectedGenres;
            ViewBag.SelectedActors = selectedActors;
            ViewBag.SelectedDirectors = selectedDirectors;

            return View(movie);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Movie movie, List<int> selectedGenres, List<int> selectedActors, List<int> selectedDirectors)
        {
            if (id != movie.Id) return NotFound();

            if (!ModelState.IsValid)
            {
                ViewBag.Genres = new MultiSelectList(await _context.Genres.ToListAsync(), "Id", "GenreName", selectedGenres);
                ViewBag.Actors = new MultiSelectList(await _context.Actors.ToListAsync(), "Id", "FullName", selectedActors);
                ViewBag.Directors = new MultiSelectList(await _context.Directors.ToListAsync(), "Id", "FullName", selectedDirectors);
                return View(movie);
            }

            try
            {
                var existingMovie = await _context.Movies
                    .Include(m => m.MovieGenres)
                    .Include(m => m.MovieCasts)
                    .Include(m => m.DirectedBies)
                    .FirstOrDefaultAsync(m => m.Id == id);

                if (existingMovie == null)
                    return NotFound();

                existingMovie.Title = movie.Title;
                existingMovie.Year = movie.Year;
                existingMovie.Description = movie.Description;
                existingMovie.MovieLength = movie.MovieLength;
                existingMovie.Poster = movie.Poster;

                _context.MovieGenres.RemoveRange(existingMovie.MovieGenres);
                existingMovie.MovieGenres = selectedGenres
                    .Select(genreId => new MovieGenre { MovieId = existingMovie.Id, GenreId = genreId })
                    .ToList();

                _context.MovieCasts.RemoveRange(existingMovie.MovieCasts);
                existingMovie.MovieCasts = selectedActors
                    .Select(actorId => new MovieCast { MovieId = existingMovie.Id, ActorId = actorId })
                    .ToList();

                _context.DirectedBies.RemoveRange(existingMovie.DirectedBies);
                existingMovie.DirectedBies = selectedDirectors
                    .Select(directorId => new DirectedBy { MovieId = existingMovie.Id, DirectorId = directorId })
                    .ToList();

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Movies.Any(m => m.Id == id)) return NotFound();
                throw;
            }
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var movie = await _context.Movies
                .Include(m => m.MovieGenres)
                .ThenInclude(mg => mg.Genre)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (movie == null) return NotFound();

            return View(movie);
        }

        // POST: Movies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var movie = _context.Movies
                .Include(m => m.MovieGenres)
                .Include(m => m.MovieCasts)
                .Include(m => m.DirectedBies)
                //.Include(m => m.Ratings)
                .FirstOrDefault(m => m.Id == id);

            if (movie != null)
            {
                _context.MovieGenres.RemoveRange(movie.MovieGenres);
                _context.MovieCasts.RemoveRange(movie.MovieCasts);
                _context.DirectedBies.RemoveRange(movie.DirectedBies);
                _context.Movies.Remove(movie);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MovieExists(int id)
        {
            return _context.Movies.Any(e => e.Id == id);
        }

        [HttpGet]
        public IActionResult Import()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Import(IFormFile fileExcel, CancellationToken cancellationToken = default)
        {
            if (fileExcel == null || fileExcel.Length == 0)
            {
                ModelState.AddModelError("", "Please select a valid Excel file.");
                return View();
            }

            var importService = _movieDataPortServiceFactory.GetImportService(fileExcel.ContentType);

            using var stream = fileExcel.OpenReadStream();
            await importService.ImportFromStreamAsync(stream, cancellationToken);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Export([FromQuery] string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
        CancellationToken cancellationToken = default)
        {
            var exportService = _movieDataPortServiceFactory.GetExportService(contentType);

            var memoryStream = new MemoryStream();

            await exportService.WriteToAsync(memoryStream, cancellationToken);

            await memoryStream.FlushAsync(cancellationToken);
            memoryStream.Position = 0;


            return new FileStreamResult(memoryStream, contentType)
            {
                FileDownloadName = $"categiries_{DateTime.UtcNow.ToShortDateString()}.xlsx"
            };
        }

        [HttpGet]
        public async Task<IActionResult> ExportDocx(CancellationToken cancellationToken)
        {
            var stream = await _reportService.GenerateSiteStatisticsReportAsync(cancellationToken);

            return File(stream,
                "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
                "Report.docx");
        }

    }
}
