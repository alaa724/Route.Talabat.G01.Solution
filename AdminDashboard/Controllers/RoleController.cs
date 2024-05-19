using AdminDashboard.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AdminDashboard.Controllers
{
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public RoleController(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task<IActionResult> Index()
        {
            // Get All Roles

            var roles = await _roleManager.Roles.ToListAsync();

            return View(roles);
        }

        [HttpPost]
        public async Task<IActionResult> Create(RoleFormViewModel model)
        {
            if (ModelState.IsValid)
            {
                var role = await _roleManager.RoleExistsAsync(model.Name);

                if (!role)
                {
                    await _roleManager.CreateAsync(new IdentityRole(model.Name.Trim()));
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError("Name", "This Role Already Exists !!");
                    return View(model);
                } 
            }
            return RedirectToAction(nameof(Index));
            
        }

        public async Task<IActionResult> Delete(string id)
        {
           var role = await _roleManager.FindByIdAsync(id);
            if (role is not null)
            {
                await _roleManager.DeleteAsync(role);
                return RedirectToAction(nameof(Index)); 
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);

            var mappedRole = new RoleViewModel()
            {
                Name = role?.Name ?? string.Empty
            };

            return View(mappedRole);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(string id , RoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                var roleExist = await _roleManager.RoleExistsAsync(model.Name);

                if (!roleExist)
                {
                    var role = await _roleManager.FindByIdAsync(id);
                    if (role is not null)
                    {
                        role.Name = model.Name;
                        await _roleManager.UpdateAsync(role);
                        return RedirectToAction(nameof(Index));
                    }
                    else
                        ModelState.AddModelError("Name", "There Is No Role!");
                }
                else
                {
                    ModelState.AddModelError("Name", "Role Is Exist");
                    return View("Index", await _roleManager.Roles.ToListAsync());
                }
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
