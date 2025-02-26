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
    public class UserRatingsController : Controller
    {
        private readonly DbCatalogContext _context;

        public UserRatingsController(DbCatalogContext context)
        {
            _context = context;
        }

        // GET: UserRatings
        public async Task<IActionResult> Index()
        {
            var dbCatalogContext = _context.UserRatings.Include(u => u.Movie).Include(u => u.User);
            return View(await dbCatalogContext.ToListAsync());
        }

        // GET: UserRatings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userRating = await _context.UserRatings
                .Include(u => u.Movie)
                .Include(u => u.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userRating == null)
            {
                return NotFound();
            }

            return View(userRating);
        }

        // GET: UserRatings/Create
        public IActionResult Create()
        {
            ViewData["MovieId"] = new SelectList(_context.Movies, "Id", "Title");
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Email");
            return View();
        }

        // POST: UserRatings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserId,MovieId,Value,Id")] UserRating userRating)
        {
            if (ModelState.IsValid)
            {
                _context.Add(userRating);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MovieId"] = new SelectList(_context.Movies, "Id", "Title", userRating.MovieId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Email", userRating.UserId);
            return View(userRating);
        }

        // GET: UserRatings/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userRating = await _context.UserRatings.FindAsync(id);
            if (userRating == null)
            {
                return NotFound();
            }
            ViewData["MovieId"] = new SelectList(_context.Movies, "Id", "Title", userRating.MovieId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Email", userRating.UserId);
            return View(userRating);
        }

        // POST: UserRatings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserId,MovieId,Value,Id")] UserRating userRating)
        {
            if (id != userRating.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(userRating);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserRatingExists(userRating.Id))
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
            ViewData["MovieId"] = new SelectList(_context.Movies, "Id", "Title", userRating.MovieId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Email", userRating.UserId);
            return View(userRating);
        }

        // GET: UserRatings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userRating = await _context.UserRatings
                .Include(u => u.Movie)
                .Include(u => u.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userRating == null)
            {
                return NotFound();
            }

            return View(userRating);
        }

        // POST: UserRatings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userRating = await _context.UserRatings.FindAsync(id);
            if (userRating != null)
            {
                _context.UserRatings.Remove(userRating);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserRatingExists(int id)
        {
            return _context.UserRatings.Any(e => e.Id == id);
        }
    }
}
