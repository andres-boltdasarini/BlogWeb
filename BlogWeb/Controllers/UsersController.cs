// Controllers/UsersController.cs
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using BlogWeb.Models;
using BlogWeb.ViewModels; // Добавьте эту using директиву

namespace BlogWeb.Controllers
{
    public class UsersController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UsersController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // GET: Users (получение всех пользователей)
        public async Task<IActionResult> Index()
        {
            var users = await _userManager.Users.ToListAsync();
            return View(users);
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: Users/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Users/Create (регистрация)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Проверка совпадения паролей
                if (model.Password != model.ConfirmPassword)
                {
                    ModelState.AddModelError("ConfirmPassword", "Пароли не совпадают");
                    return View(model);
                }

                var user = new ApplicationUser
                {
                    UserName = model.UserName,
                    Email = model.Email,
                    DisplayName = model.DisplayName,
                    Bio = model.Bio,
                    AvatarUrl = model.AvatarUrl,
                    IsActive = model.IsActive,
                    EmailConfirmed = true // или false, в зависимости от вашей логики
                };

                var result = await _userManager.CreateAsync(user, model.Password);
                
                if (result.Succeeded)
                {
                    // Добавляем базовую роль "Пользователь"
                    if (!await _roleManager.RoleExistsAsync("Пользователь"))
                    {
                        await _roleManager.CreateAsync(new IdentityRole("Пользователь"));
                    }
                    
                    await _userManager.AddToRoleAsync(user, "Пользователь");
                    return RedirectToAction(nameof(Index));
                }
                
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            
            return View(model);
        }
    }
}