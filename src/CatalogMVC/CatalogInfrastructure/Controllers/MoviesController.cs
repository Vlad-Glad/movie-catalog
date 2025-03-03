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
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movies
                .Include(m => m.MovieGenres)
                .ThenInclude(mg => mg.Genre)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        public IActionResult Create()
        {
            ViewData["Genres"] = new SelectList(_context.Genres, "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Movie movie, List<int> selectedGenres)
        {
            if (ModelState.IsValid)
            {
                _context.Add(movie);
                await _context.SaveChangesAsync();

                if (selectedGenres != null)
                {
                    foreach (var genreId in selectedGenres)
                    {
                        _context.MovieGenres.Add(new MovieGenre { MovieId = movie.Id, GenreId = genreId });
                    }
                    await _context.SaveChangesAsync();
                }

                return RedirectToAction(nameof(Index));
            }
            ViewData["Genres"] = new SelectList(_context.Genres, "Id", "Name", selectedGenres);
            return View(movie);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            // Load movie with its genres
            var movie = await _context.Movies
                .Include(m => m.MovieGenres)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (movie == null) return NotFound();

            // Get selected genres for this movie
            var selectedGenres = movie.MovieGenres.Select(mg => mg.GenreId).ToList();

            // Pass all genres to the view and preselect the existing ones
            ViewData["Genres"] = new SelectList(await _context.Genres.ToListAsync(), "Id", "Name");

            ViewBag.SelectedGenres = selectedGenres; // Send selected genres to the view

            return View(movie);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Movie movie, List<int> selectedGenres)
        {
            if (id != movie.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Retrieve the existing movie with its genres
                    var existingMovie = await _context.Movies
                        .Include(m => m.MovieGenres)
                        .FirstOrDefaultAsync(m => m.Id == id);

                    if (existingMovie == null)
                    {
                        return NotFound();
                    }

                    existingMovie.Title = movie.Title;
                    existingMovie.Year = movie.Year;
                    existingMovie.Description = movie.Description;
                    existingMovie.MovieLength = movie.MovieLength;
                    existingMovie.Poster = movie.Poster;

                    _context.MovieGenres.RemoveRange(existingMovie.MovieGenres);
                    await _context.SaveChangesAsync();

                    if (selectedGenres != null)
                    {
                        foreach (var genreId in selectedGenres)
                        {
                            _context.MovieGenres.Add(new MovieGenre { MovieId = existingMovie.Id, GenreId = genreId });
                        }
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

            // Reload genres in case of an error
            ViewData["Genres"] = new SelectList(await _context.Genres.ToListAsync(), "Id", "Name");
            ViewBag.SelectedGenres = selectedGenres;

            return View(movie);
        }


        // GET: Movies/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movies
                .FirstOrDefaultAsync(m => m.Id == id);
            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        // POST: Movies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var movie = await _context.Movies.FindAsync(id);
            if (movie != null)
            {
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
