using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CatalogDomain.Model;
using CatalogInfrastructure;

namespace CatalogInfrastructure.Controllers
{
    public class MoviesController : Controller
    {
        private readonly DbCatalogContext _context;

        public MoviesController(DbCatalogContext context)
        {
            _context = context;
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
                .FirstOrDefaultAsync(m => m.Id == id);

            if (movie == null) return NotFound();

            return View(movie);
        }

        public IActionResult Create()
        {
            ViewBag.Genres = new MultiSelectList(_context.Genres.ToList(), "Id", "GenreName");

            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Movie movie, List<int> selectedGenres)
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

                return RedirectToAction(nameof(Index));
            }

            ViewBag.Genres = new MultiSelectList(await _context.Genres.ToListAsync(), "Id", "GenreName", selectedGenres);
            return View(movie);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var movie = await _context.Movies
                .Include(m => m.MovieGenres)
                .ThenInclude(mg => mg.Genre)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (movie == null) return NotFound();

            var selectedGenres = movie.MovieGenres.Select(mg => mg.GenreId).ToList();

            ViewData["Genres"] = new SelectList(await _context.Genres.ToListAsync(), "Id", "GenreName");
            ViewBag.SelectedGenres = selectedGenres;
            return View(movie);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Movie movie, List<int> selectedGenres)
        {
            if (id != movie.Id) return NotFound();

            if (!ModelState.IsValid)
            {
                ViewData["Genres"] = new SelectList(await _context.Genres.ToListAsync(), "Id", "GenreName");
                ViewBag.SelectedGenres = selectedGenres;
                return View(movie);
            }

            try
            {
                var existingMovie = await _context.Movies
                    .Include(m => m.MovieGenres)
                    .FirstOrDefaultAsync(m => m.Id == id);

                if (existingMovie == null) return NotFound();

                existingMovie.Title = movie.Title;
                existingMovie.Year = movie.Year;
                existingMovie.Description = movie.Description;
                existingMovie.MovieLength = movie.MovieLength;
                existingMovie.Poster = movie.Poster;

                var existingMovieGenres = await _context.MovieGenres
                    .Where(mg => mg.MovieId == id)
                    .ToListAsync();

                var existingGenreIds = existingMovieGenres.Select(mg => mg.GenreId).ToList();

                var genresToRemove = existingMovieGenres
                    .Where(mg => !selectedGenres.Contains(mg.GenreId))
                    .ToList();

                if (genresToRemove.Any())
                {
                    _context.MovieGenres.RemoveRange(genresToRemove);
                }

                var newGenresToAdd = selectedGenres
                    .Where(genreId => !existingGenreIds.Contains(genreId))
                    .Select(genreId => new MovieGenre { MovieId = existingMovie.Id, GenreId = genreId })
                    .ToList();

                if (newGenresToAdd.Any())
                {
                    await _context.MovieGenres.AddRangeAsync(newGenresToAdd);
                }

                if (genresToRemove.Any() || newGenresToAdd.Any())
                {
                    await _context.SaveChangesAsync();
                }

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
                .FirstOrDefault(m => m.Id == id);

            if (movie != null)
            {
                _context.MovieGenres.RemoveRange(movie.MovieGenres);
                _context.Movies.Remove(movie);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MovieExists(int id)
        {
            return _context.Movies.Any(e => e.Id == id);
        }
    }
}
