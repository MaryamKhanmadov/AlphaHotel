using Alpha_Hotel_Project.Areas.Manage.ViewModels;
using Alpha_Hotel_Project.Data;
using Alpha_Hotel_Project.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Alpha_Hotel_Project.Areas.Manage.Controllers
{
    [Area("Manage")]
    [Authorize(Roles = "SuperAdmin")]
    public class AdminController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly AppDbContext _appDbContext;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AdminController(UserManager<AppUser> userManager, AppDbContext appDbContext, RoleManager<IdentityRole> roleManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _appDbContext = appDbContext;
            _roleManager = roleManager;
            _signInManager = signInManager;
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(AdminRegisterViewModel adminRegisterVM)
        {
            if (!ModelState.IsValid) return View();
            AppUser appUser = null;
            appUser = await _userManager.FindByNameAsync(adminRegisterVM.Username);
            if (appUser != null)
            {
                ModelState.AddModelError("Username", "Already exist!");
                return View();
            }
            appUser = _appDbContext.Users.FirstOrDefault(x => x.NormalizedEmail == adminRegisterVM.Email.ToUpper());
            if (appUser != null)
            {
                ModelState.AddModelError("Email", "Already exist!");
                return View();
            }
            appUser = new AppUser
            {
                Fullname = adminRegisterVM.Fullname,
                Email = adminRegisterVM.Email,
                UserName = adminRegisterVM.Username,
            };
            var result = await _userManager.CreateAsync(appUser, adminRegisterVM.Password);
            if (!result.Succeeded)
            {
                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            await _userManager.AddToRoleAsync(appUser, "Admin");
            await _signInManager.SignInAsync(appUser, isPersistent: false);
            return RedirectToAction("Index", "Dashboard");
        }
    }
}
