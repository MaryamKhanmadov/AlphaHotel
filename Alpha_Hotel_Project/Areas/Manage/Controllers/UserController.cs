using Alpha_Hotel_Project.Data;
using Alpha_Hotel_Project.Helpers;
using Alpha_Hotel_Project.Models;
using Alpha_Hotel_Project.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Security.Cryptography.Xml;

namespace Alpha_Hotel_Project.Areas.Manage.Controllers
{
    [Area("Manage")]
    [Authorize(Roles = "SuperAdmin,Admin")]
    public class UserController : Controller
    {
        private readonly AppDbContext _appDbContext;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<AppUser> _userManager;

        public UserController(AppDbContext appDbContext, UserManager<AppUser> userManager)
        {
            _appDbContext = appDbContext;
            _userManager = userManager;
        }
        public IActionResult Index(string search, int page = 1)
        {
            var query = search != null ?
                _userManager.Users.Where(x => x.Fullname.ToLower().Contains(search.ToLower()) || x.UserName.ToLower().Contains(search.ToLower()) || x.Email.ToLower().Contains(search.ToLower())).AsQueryable() :
                _userManager.Users.AsQueryable();
            PaginatedList<AppUser> Users = PaginatedList<AppUser>.Create(query, 5, page);
            return View(Users);
        }
        public IActionResult Actived(Guid id)
        {
            AppUser appUser = _appDbContext.Users.Find(id.ToString());
            if (appUser == null) return View("Error");
            appUser.IsActive = true;
            _appDbContext.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult Blocked(Guid id)
        {
            AppUser appUser = _appDbContext.Users.Find(id.ToString());
            if (appUser == null) return View("Error");
            appUser.IsActive = false;
            _appDbContext.SaveChanges();
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Detail(Guid id)
        {
            AppUser appUser = await _userManager.FindByIdAsync(id.ToString());
            if (appUser == null) return View("Error");
            return View(new UserDetailViewModel
            {
                User = appUser,
                UserRoles = await _userManager.GetRolesAsync(appUser)
            });
        }
    }
}
