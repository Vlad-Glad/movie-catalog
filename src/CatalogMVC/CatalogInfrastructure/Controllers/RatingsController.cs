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
    public class RatingsController : Controller
    {
        private readonly DbCatalogContext _context;

        public RatingsController(DbCatalogContext context)
        {
            _context = context;
        }

        // GET: Ratings
        public async Task<IActionResult> Index()
        {
            var dbCatalogContext = _context.Ratings.Include(r => r.Movie);
            return View(await dbCatalogContext.ToListAsync());
        }

        // GET: Ratings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rating = await _context.Ratings
                .Include(r => r.Movie)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (rating == null)
            {
                return NotFound();
            }

            return View(rating);
        }

        // GET: Ratings/Create
        public IActionResult Create()
        {
            ViewData["MovieId"] = new SelectList(_context.Movies, "Id", "Title");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MovieId,Source,Value")] Rating rating)
        {
            if (ModelState.IsValid)
            {
                bool movieExists = await _context.Movies.AnyAsync(m => m.Id == rating.MovieId);
                
                if (!movieExists)
                {
                    ModelState.AddModelError("MovieId", "Selected movie doesn't exist");
                    ViewData["MovieId"] = new SelectList(_context.Movies, "Id", "Title", rating.MovieId);
                    return View(rating);
                }

                bool duplicateSourceExists = await _context.Ratings
                        .AnyAsync(r => r.MovieId == rating.MovieId && r.Source.ToLower() == rating.Source.ToLower());

                if (duplicateSourceExists)
                {
                    ModelState.AddModelError("Source", "This source already exists for the selected movie.");
                    ViewData["MovieId"] = new SelectList(_context.Movies, "Id", "Title", rating.MovieId);
                    return View(rating);
                }

                rating.Movie = await _context.Movies.FindAsync(rating.MovieId);
                _context.Add(rating);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index)); 
            }

            ViewData["MovieId"] = new SelectList(_context.Movies, "Id", "Title", rating.MovieId);
            return View(rating);
        }

        // GET: Ratings/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rating = await _context.Ratings.FindAsync(id);
            if (rating == null)
            {
                return NotFound();
            }
            ViewData["MovieId"] = new SelectList(_context.Movies, "Id", "Title", rating.MovieId);
            return View(rating);
        }

        // POST: Ratings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MovieId,Source,Value,Id")] Rating rating)
        {
            if (id != rating.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                bool movieExists = await _context.Movies.AnyAsync(m => m.Id == rating.MovieId);
                if (!movieExists)
                {
                    ModelState.AddModelError("MovieId", "Selected movie doesn't exist");
                    ViewData["MovieId"] = new SelectList(_context.Movies, "Id", "Title", rating.MovieId);
                    return View(rating);
                }

                bool duplicateSourceExists = await _context.Ratings
                    .AnyAsync(r => r.MovieId == rating.MovieId &&
                                   r.Id != rating.Id &&
                                   r.Source.ToLower() == rating.Source.ToLower());

                if (duplicateSourceExists)
                {
                    ModelState.AddModelError("Source", "This source already exists for the selected movie.");
                    ViewData["MovieId"] = new SelectList(_context.Movies, "Id", "Title", rating.MovieId);
                    return View(rating);
                }
                try
                {
                    _context.Update(rating);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RatingExists(rating.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["MovieId"] = new SelectList(_context.Movies, "Id", "Title", rating.MovieId);
            return View(rating);
        }

        // GET: Ratings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rating = await _context.Ratings
                .Include(r => r.Movie)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (rating == null)
            {
                return NotFound();
            }

            return View(rating);
        }

        // POST: Ratings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var rating = await _context.Ratings.FindAsync(id);
            if (rating != null)
            {
                _context.Ratings.Remove(rating);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RatingExists(int id)
        {
            return _context.Ratings.Any(e => e.Id == id);
        }
    }
}
