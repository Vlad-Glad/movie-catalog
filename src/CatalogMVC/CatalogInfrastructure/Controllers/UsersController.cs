using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CatalogDomain.Model;
using CatalogInfrastructure.Services;
using CatalogInfrastructure;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.General;

namespace CatalogInfrastructure.Controllers
{
    public class UsersController : Controller
    {
        //private readonly DbCatalogContext _context;

        //public UsersController(DbCatalogContext context)
        //{
        //    _context = context;
        //}

        //// GET: Users
        //public async Task<IActionResult> Index()
        //{
        //    return View(await _context.Users.ToListAsync());
        //}

        //// GET: Users/Details/5
        //public async Task<IActionResult> Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var user = await _context.Users
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (user == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(user);
        //}

        //// GET: Users/Create
        //public IActionResult Create()
        //{
        //    return View();
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("Email,Password,ConfirmPassword,Id")] User user)
        //{
        //    user.Role = CatalogDomain.Model.User.OrdinaryUser;

        //    if (user.Password != user.ConfirmPassword)
        //    {
        //        ModelState.AddModelError("ConfirmPassword", "Passwords do not match");
        //        return View(user);
        //    }

        //    var passwordService = new PasswordService();
        //    user.PasswordHash = passwordService.HashPassword(user.Password);

        //    _context.Add(user);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        //// GET: Users/Edit/5
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var user = await _context.Users.FindAsync(id);
        //    if (user == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(user);
        //}


        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("Email,Password,ConfirmPassword,Id")] User updatedUser)
        //{
        //    if (id != updatedUser.Id)
        //        return NotFound();

        //    var existingUser = await _context.Users.FindAsync(id);
        //    if (existingUser == null)
        //        return NotFound();

        //    existingUser.Email = updatedUser.Email;

        //    if (!string.IsNullOrWhiteSpace(updatedUser.Password) || !string.IsNullOrWhiteSpace(updatedUser.ConfirmPassword))
        //    {
        //        if (updatedUser.Password != updatedUser.ConfirmPassword)
        //        {
        //            ModelState.AddModelError("ConfirmPassword", "Passwords do not match");
        //            return View(updatedUser);
        //        }

        //        var passwordService = new PasswordService();
        //        existingUser.PasswordHash = passwordService.HashPassword(updatedUser.Password);
        //    }

        //    try
        //    {
        //        _context.Update(existingUser);
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!UserExists(updatedUser.Id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return RedirectToAction(nameof(Index));
        //}


        //// GET: Users/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var user = await _context.Users
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (user == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(user);
        //}

        //// POST: Users/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var user = await _context.Users.FindAsync(id);
        //    if (user != null)
        //    {
        //        _context.Users.Remove(user);
        //    }

        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        //private bool UserExists(int id)
        //{
        //    return _context.Users.Any(e => e.Id == id);
        //}
    }
}
