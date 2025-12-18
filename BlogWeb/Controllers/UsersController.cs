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

// GET: Users/Edit/5
public async Task<IActionResult> Edit(string id)
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

    // Преобразуем в ViewModel
    var model = new EditUserViewModel
    {
        Id = user.Id,
        UserName = user.UserName,
        Email = user.Email,
        DisplayName = user.DisplayName,
        Bio = user.Bio,
        AvatarUrl = user.AvatarUrl,
        PhoneNumber = user.PhoneNumber,
        EmailConfirmed = user.EmailConfirmed,
        PhoneNumberConfirmed = user.PhoneNumberConfirmed,
        TwoFactorEnabled = user.TwoFactorEnabled,
        IsActive = user.IsActive,
        LockoutEnd = user.LockoutEnd,
        AccessFailedCount = user.AccessFailedCount
    };

    return View(model);
}

// POST: Users/Edit/5
[HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> Edit(string id, EditUserViewModel model)
{
    if (id != model.Id)
    {
        return NotFound();
    }

    if (ModelState.IsValid)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null)
        {
            return NotFound();
        }

        // Обновляем свойства пользователя
        user.UserName = model.UserName;
        user.Email = model.Email;
        user.DisplayName = model.DisplayName;
        user.Bio = model.Bio;
        user.AvatarUrl = model.AvatarUrl;
        user.PhoneNumber = model.PhoneNumber;
        user.EmailConfirmed = model.EmailConfirmed;
        user.PhoneNumberConfirmed = model.PhoneNumberConfirmed;
        user.TwoFactorEnabled = model.TwoFactorEnabled;
        user.IsActive = model.IsActive;

        // Управление блокировкой
        if (model.ClearLockout)
        {
            user.LockoutEnd = null;
            user.AccessFailedCount = 0;
        }
        else if (model.LockoutEnd.HasValue)
        {
            user.LockoutEnd = model.LockoutEnd;
        }

        // Сброс счетчика неудачных попыток
        user.AccessFailedCount = model.AccessFailedCount;

        // Обновление пользователя
        var result = await _userManager.UpdateAsync(user);
        
        if (result.Succeeded)
        {
            // Обновление Security Stamp (разлогинивает пользователя)
            if (model.ResetSecurityStamp)
            {
                await _userManager.UpdateSecurityStampAsync(user);
            }

            TempData["SuccessMessage"] = $"Пользователь {user.UserName} успешно обновлен!";
            return RedirectToAction(nameof(Details), new { id = user.Id });
        }
        
        // Если есть ошибки, добавляем их в ModelState
        foreach (var error in result.Errors)
        {
            ModelState.AddModelError(string.Empty, error.Description);
        }
    }
    
    return View(model);
}

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(string id)
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

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                // Вместо удаления можно деактивировать пользователя
                user.IsActive = false;
                await _userManager.UpdateAsync(user);
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: Users/GetUserRoles/5
        public async Task<IActionResult> GetUserRoles(string id)
        {
            if (id == null)
            {
                return Json(new List<string>());
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return Json(new List<string>());
            }

            var roles = await _userManager.GetRolesAsync(user);
            return Json(roles);
        }

        // GET: Users/GetAllRoles
        public async Task<IActionResult> GetAllRoles()
        {
            var roles = await _roleManager.Roles
                .Select(r => new { r.Id, r.Name })
                .ToListAsync();
            
            return Json(roles);
        }

        // POST: Users/ToggleRole/{userId}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleRole(string userId, [FromBody] RoleToggleRequest request)
        {
            if (string.IsNullOrEmpty(userId) || request == null)
            {
                return BadRequest("Invalid request");
            }
            
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound("User not found");
            }
            
            if (request.IsAdd)
            {
                // Добавляем роль
                if (!await _roleManager.RoleExistsAsync(request.RoleName))
                {
                    await _roleManager.CreateAsync(new IdentityRole(request.RoleName));
                }
                
                var result = await _userManager.AddToRoleAsync(user, request.RoleName);
                if (!result.Succeeded)
                {
                    return BadRequest(result.Errors);
                }
            }
            else
            {
                // Удаляем роль
                var result = await _userManager.RemoveFromRoleAsync(user, request.RoleName);
                if (!result.Succeeded)
                {
                    return BadRequest(result.Errors);
                }
            }
            
            return Ok();
        }

        // POST: Users/ChangePassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(string id, [FromBody] ChangePasswordRequest request)
        {
            if (string.IsNullOrEmpty(id) || request == null)
            {
                return BadRequest();
            }
            
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            
            // Генерируем токен сброса пароля
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            
            // Сбрасываем пароль
            var result = await _userManager.ResetPasswordAsync(user, token, request.NewPassword);
            
            if (result.Succeeded)
            {
                // Разлогиниваем пользователя на всех устройствах
                await _userManager.UpdateSecurityStampAsync(user);
                return Ok();
            }
            
            return BadRequest(result.Errors);
        }

        // POST: Users/Activate/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Activate(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                user.IsActive = true;
                await _userManager.UpdateAsync(user);
            }
            return RedirectToAction(nameof(Details), new { id });
        }

        // Вспомогательные методы

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        private async Task<bool> UserExists(string id)
        {
            return await _userManager.FindByIdAsync(id) != null;
        }
    }

    // Классы для запросов

    public class RoleToggleRequest
    {
        public string RoleName { get; set; }
        public bool IsAdd { get; set; }
    }

    public class ChangePasswordRequest
    {
        public string NewPassword { get; set; }
    }
}