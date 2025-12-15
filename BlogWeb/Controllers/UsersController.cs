using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using BlogWeb.Models;

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

public async Task<IActionResult> Index(){
    var users = await _userManager.Users.ToListAsync();
    return View(users);
}
    }
}