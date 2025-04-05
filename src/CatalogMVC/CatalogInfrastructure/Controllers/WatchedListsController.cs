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
    public class WatchedListsController : Controller
    {
        private readonly DbCatalogContext _context;

        public WatchedListsController(DbCatalogContext context)
        {
            _context = context;
        }

        // GET: WatchedLists
        public async Task<IActionResult> Index()
        {
            var dbCatalogContext = _context.WatchedLists.Include(w => w.Movie).Include(w => w.User);
            return View(await dbCatalogContext.ToListAsync());
        }

        // GET: WatchedLists/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var watchedList = await _context.WatchedLists
                .Include(w => w.Movie)
                .Include(w => w.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (watchedList == null)
            {
                return NotFound();
            }

            return View(watchedList);
        }

        // GET: WatchedLists/Create
        public IActionResult Create()
        {
            ViewData["MovieId"] = new SelectList(_context.Movies, "Id", "Title");
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Email");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserId,MovieId,WatchedDate,Id")] WatchedList watchedList)
        {
            if (ModelState.IsValid)
            {
                bool alreadyExists = await _context.WatchedLists
                        .AnyAsync(w => w.UserId == watchedList.UserId && w.MovieId == watchedList.MovieId);

                if (alreadyExists)
                {
                    ModelState.AddModelError(string.Empty, "This movie is already in the user's watch list.");
                    ViewData["MovieId"] = new SelectList(_context.Movies, "Id", "Title", watchedList.MovieId);
                    return View(watchedList);
                }


                _context.Add(watchedList);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MovieId"] = new SelectList(_context.Movies, "Id", "Title", watchedList.MovieId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Email", watchedList.UserId);
            return View(watchedList);
        }

        // GET: WatchedLists/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var watchedList = await _context.WatchedLists.FindAsync(id);
            if (watchedList == null)
            {
                return NotFound();
            }
            ViewData["MovieId"] = new SelectList(_context.Movies, "Id", "Title", watchedList.MovieId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Email", watchedList.UserId);
            return View(watchedList);
        }

        // POST: WatchedLists/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserId,MovieId,WatchedDate,Id")] WatchedList watchedList)
        {
            if (id != watchedList.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(watchedList);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WatchedListExists(watchedList.Id))
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
            ViewData["MovieId"] = new SelectList(_context.Movies, "Id", "Title", watchedList.MovieId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Email", watchedList.UserId);
            return View(watchedList);
        }

        // GET: WatchedLists/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var watchedList = await _context.WatchedLists
                .Include(w => w.Movie)
                .Include(w => w.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (watchedList == null)
            {
                return NotFound();
            }

            return View(watchedList);
        }

        // POST: WatchedLists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var watchedList = await _context.WatchedLists.FindAsync(id);
            if (watchedList != null)
            {
                _context.WatchedLists.Remove(watchedList);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool WatchedListExists(int id)
        {
            return _context.WatchedLists.Any(e => e.Id == id);
        }
    }
}
