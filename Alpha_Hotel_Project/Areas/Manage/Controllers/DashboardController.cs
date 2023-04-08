using Alpha_Hotel_Project.Areas.Manage.ViewModels;
using Alpha_Hotel_Project.Data;
using Alpha_Hotel_Project.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Alpha_Hotel_Project.Areas.Manage.Controllers
{
    [Area("Manage")]
    [Authorize(Roles = "SuperAdmin,Admin")]
    public class DashboardController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly AppDbContext _appDbContext;

        public DashboardController(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager , AppDbContext appDbContext)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _appDbContext = appDbContext;
        }
        public IActionResult Index()
        {
            DashboardViewModel dashboardViewModel = new DashboardViewModel
            {
                OrderItems = _appDbContext.OrderItems.Include(x=>x.Order).ToList(),
                Orders = _appDbContext.Orders.Include(x => x.OrderItem).ToList(),
                AppUsers = _appDbContext.Users.ToList(),
                RecentOrders = _appDbContext.Orders.Include(x => x.OrderItem).OrderByDescending(x => x.CreateDate).Where(x => x.OrderStatus == 0).Take(5).ToList(),
                ContactMessages = _appDbContext.ContactMessages.OrderByDescending(x=>x.MessageTime).Take(3).ToList()
            };
            return View(dashboardViewModel);
        }
        //public async Task<IActionResult> CreateAdmin()
        //{
        //    AppUser admin = new AppUser
        //    {
        //        UserName = "Maryam",
        //        Fullname = "Maryam Khanmadova"
        //    };

        //    var result = await _userManager.CreateAsync(admin, "Admin123");
        //    return Ok(result);
        //}

        //public async Task<IActionResult> CreateRole()
        //{
        //    IdentityRole role1 = new IdentityRole("SuperAdmin");
        //    IdentityRole role2 = new IdentityRole("Admin");
        //    IdentityRole role3 = new IdentityRole("Member");

        //    await _roleManager.CreateAsync(role1);
        //    await _roleManager.CreateAsync(role2);
        //    await _roleManager.CreateAsync(role3);

        //    return Ok("Success");
        //}

        public async Task<IActionResult> AddRole()
        {
            AppUser appUser = await _userManager.FindByNameAsync("Maryam");

            await _userManager.AddToRoleAsync(appUser, "SuperAdmin");

            return Ok("Role Added");
        }
    }
}
