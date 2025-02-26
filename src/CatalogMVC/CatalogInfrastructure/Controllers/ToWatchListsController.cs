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
    public class ToWatchListsController : Controller
    {
        private readonly DbCatalogContext _context;

        public ToWatchListsController(DbCatalogContext context)
        {
            _context = context;
        }

        // GET: ToWatchLists
        public async Task<IActionResult> Index()
        {
            var dbCatalogContext = _context.ToWatchLists.Include(t => t.Movie).Include(t => t.User);
            return View(await dbCatalogContext.ToListAsync());
        }

        // GET: ToWatchLists/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var toWatchList = await _context.ToWatchLists
                .Include(t => t.Movie)
                .Include(t => t.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (toWatchList == null)
            {
                return NotFound();
            }

            return View(toWatchList);
        }

        // GET: ToWatchLists/Create
        public IActionResult Create()
        {
            ViewData["MovieId"] = new SelectList(_context.Movies, "Id", "Title");
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Email");
            return View();
        }

        // POST: ToWatchLists/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserId,MovieId,AddedDate,Id")] ToWatchList toWatchList)
        {
            if (ModelState.IsValid)
            {
                _context.Add(toWatchList);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MovieId"] = new SelectList(_context.Movies, "Id", "Title", toWatchList.MovieId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Email", toWatchList.UserId);
            return View(toWatchList);
        }

        // GET: ToWatchLists/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var toWatchList = await _context.ToWatchLists.FindAsync(id);
            if (toWatchList == null)
            {
                return NotFound();
            }
            ViewData["MovieId"] = new SelectList(_context.Movies, "Id", "Title", toWatchList.MovieId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Email", toWatchList.UserId);
            return View(toWatchList);
        }

        // POST: ToWatchLists/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserId,MovieId,AddedDate,Id")] ToWatchList toWatchList)
        {
            if (id != toWatchList.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(toWatchList);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ToWatchListExists(toWatchList.Id))
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
            ViewData["MovieId"] = new SelectList(_context.Movies, "Id", "Title", toWatchList.MovieId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Email", toWatchList.UserId);
            return View(toWatchList);
        }

        // GET: ToWatchLists/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var toWatchList = await _context.ToWatchLists
                .Include(t => t.Movie)
                .Include(t => t.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (toWatchList == null)
            {
                return NotFound();
            }

            return View(toWatchList);
        }

        // POST: ToWatchLists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var toWatchList = await _context.ToWatchLists.FindAsync(id);
            if (toWatchList != null)
            {
                _context.ToWatchLists.Remove(toWatchList);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ToWatchListExists(int id)
        {
            return _context.ToWatchLists.Any(e => e.Id == id);
        }
    }
}
