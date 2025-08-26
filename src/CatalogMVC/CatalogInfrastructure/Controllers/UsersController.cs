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
using Microsoft.Build.Framework;
using Microsoft.AspNetCore.Identity;

namespace CatalogInfrastructure.Controllers
{
    public class UsersController : Controller
    {
        private readonly UserManager<AspNetCoreUser> _userManager;

        public UsersController(UserManager<AspNetCoreUser> userManager)
        {
            _userManager = userManager;
        }

        // GET: Users
        public async Task<IActionResult> Index()
        {
            var users = await _userManager.Users.ToListAsync();

            return View(users);
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(string? id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return NotFound();
            }

            var user = await _userManager.Users
                .FirstOrDefaultAsync(m => m.Id == id);

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        public IActionResult Create()
        {
            return View();
        }

        public class CreateUserVm
        {
            public string Email { get; set; }
            public string UserName { get; set; }
            public string Password { get; set; }
            public string ConfirmPassword { get; set; }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateUserVm vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            if (vm.Password != vm.ConfirmPassword)
            {
                ModelState.AddModelError(nameof(vm.ConfirmPassword), "Password do not match");
                return View(vm);
            }

            var user = new AspNetCoreUser
            {
                UserName = string.IsNullOrWhiteSpace(vm.UserName) ? vm.Email : vm.UserName,
                Email = vm.Email,
                EmailConfirmed = true,
            };

            var result = await _userManager.CreateAsync(user, vm.Password);
            if (!result.Succeeded)
            {
                foreach (var e in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, e.Description);
                    return View(vm);
                }
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(string? id)
        {

            if (string.IsNullOrWhiteSpace(id))
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            var vm = new EditUserVm { Id = user.Id, Email = user.Email, UserName = user.UserName};

            return View(vm);
        }

        public class EditUserVm
        {
            public string Id { get; set; }
            public string Email { get; set; }
            public string UserName { get; set; }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, EditUserVm vm)
        {
            if (id != vm.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            user.Email = vm.Email;
            user.UserName = vm.UserName;

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                foreach (var e in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, e.Description);

                    return View(vm);
                }
            }

            return RedirectToAction(nameof(Index));
        }


        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(string? id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return NotFound();

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return NotFound();

            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user != null)
            {
                var result = await _userManager.DeleteAsync(user);

                if (!result.Succeeded)
                {
                    foreach (var e in result.Errors)
                        ModelState.AddModelError(string.Empty, e.Description);
                    return View("Delete", user);
                }
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
