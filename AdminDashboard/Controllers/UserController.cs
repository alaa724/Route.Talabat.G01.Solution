using AdminDashboard.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Talabat.Core.Entities.Identity;

namespace AdminDashboard.Controllers
{
    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUsers> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserController(
            UserManager<ApplicationUsers> userManager , 
            RoleManager<IdentityRole> roleManager
            )
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<IActionResult> Index()
        {
            var usersData = await _userManager.Users.ToListAsync();
            var users = usersData.Select(U => new UserViewModel()
            {
                Id = U.Id,
                UserName = U.UserName,
                DisplayName = U.DisplayName,
                Email = U.Email,
                Roles = _userManager.GetRolesAsync(U).Result,

            }).ToList();
            

            return View(users);
        }

        public async Task<IActionResult> Edit(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            var allRoles = await _roleManager.Roles.ToListAsync();

            var model = new UserRoleViewModel()
            {
                UserId = user.Id,
                UserName = user.UserName,
                Roles = allRoles.Select(R => new RoleViewModel()
                {
                    Id = R.Id,
                    Name = R.Name,
                    IsSelected = _userManager.IsInRoleAsync(user, R.Name).Result
                }).ToList()
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UserRoleViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);

            var userRole = await _userManager.GetRolesAsync(user);

            foreach(var role in model.Roles)
            {
                if (userRole.Any(R => R == role.Name) && !role.IsSelected)
                    await _userManager.RemoveFromRoleAsync(user, role.Name);
                if (!userRole.Any(R => R == role.Name) && role.IsSelected)
                    await _userManager.AddToRoleAsync(user, role.Name);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
