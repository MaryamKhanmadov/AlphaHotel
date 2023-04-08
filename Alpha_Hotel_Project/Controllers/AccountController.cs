using Alpha_Hotel_Project.Data;
using Alpha_Hotel_Project.Helpers;
using Alpha_Hotel_Project.Models;
using Alpha_Hotel_Project.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using static NuGet.Packaging.PackagingConstants;

namespace Alpha_Hotel_Project.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly AppDbContext _appDbContext;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AccountController(UserManager<AppUser> userManager, AppDbContext appDbContext, RoleManager<IdentityRole> roleManager, SignInManager<AppUser> signInManager)
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
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Register(MemberRegisterViewModel memberRegisterVM)
        {
            if (!ModelState.IsValid) return View();
            AppUser appUser = null;
            appUser = await _userManager.FindByNameAsync(memberRegisterVM.Username);
            if (appUser != null)
            {
                ModelState.AddModelError("Username", "Already exist!");
                return View();
            }
            appUser = _appDbContext.Users.FirstOrDefault(x => x.NormalizedEmail == memberRegisterVM.Email.ToUpper());

            if (appUser != null)
            {
                ModelState.AddModelError("Email", "Already exist!");
                return View();
            }
            appUser = new AppUser
            {
                Fullname = memberRegisterVM.Fullname,
                Email = memberRegisterVM.Email,
                UserName = memberRegisterVM.Username,
                IsActive = true
            };
            var result = await _userManager.CreateAsync(appUser, memberRegisterVM.Password);
            if (!result.Succeeded)
            {
                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View(memberRegisterVM);
            }
            await _userManager.AddToRoleAsync(appUser, "Member");
            //await _signInManager.SignInAsync(appUser, isPersistent: false);
            return RedirectToAction("login");
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Login(MemberLoginViewModel memberLoginVM)
        {
            if (!ModelState.IsValid) return View();
            AppUser appUser = await _userManager.FindByNameAsync(memberLoginVM.Username);
            if (appUser is null)
            {
                ModelState.AddModelError("", "Username or password is incorrect");
                return View(memberLoginVM);
            }
            if (!appUser.IsActive)
            {
                ModelState.AddModelError("", "Account is blocked!");
                return View(memberLoginVM);
            }
            var result = await _signInManager.PasswordSignInAsync(appUser, memberLoginVM.Password, memberLoginVM.RememberMe, true);
            if (result.IsLockedOut)
            {
                ModelState.AddModelError("", "Account is blocked!");
                return View(memberLoginVM);
            }
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Username or password is incorrect");
                return View(memberLoginVM);
            }
            await _signInManager.SignInAsync(appUser, memberLoginVM.RememberMe);
            return RedirectToAction("Index", "Home");
        }
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction("index", "home");
        }
        public async Task<IActionResult> Profile(int page = 1)
        {
            AppUser member = null;
            if (User.Identity.IsAuthenticated)
            {
                member = await _userManager.FindByNameAsync(User.Identity.Name);
            }
            var query = _appDbContext.Orders.Include(x => x.OrderItem).Where(x => x.OrderItem.EndRentDate.DayOfYear > DateTime.Now.DayOfYear).Where(x => x.AppUserId == member.Id).Where(x => x.IsCancel == false).AsQueryable();
            PaginatedList<Order> orders = PaginatedList<Order>.Create(query, 5, page);
            return View(orders);
        }
        public async Task<IActionResult> CancelIndex(int page = 1)
        {
            AppUser member = null;
            if (User.Identity.IsAuthenticated)
            {
                member = await _userManager.FindByNameAsync(User.Identity.Name);
            }
            var query = _appDbContext.Orders.Include(x => x.OrderItem).Where(x => x.AppUserId == member.Id).Where(x => x.IsCancel == true).AsQueryable();
            PaginatedList<Order> orders = PaginatedList<Order>.Create(query, 5, page);
            return View(orders);
        }
        public async Task<IActionResult> Cancel(Guid id)
        {
            Order order = _appDbContext.Orders.FirstOrDefault(x => x.Id == id);
            if (order == null) return View("Error");
            int dayCount = DateTime.Now.DayOfYear - order.CreateDate.DayOfYear;
            if (dayCount > 1 || order.OrderStatus == Enums.OrderStatus.Accepted)
            {
                ModelState.AddModelError("", "It is possible to cancel within one day only");
                return View("profile");
            }
            order.IsCancel = true;
            _appDbContext.SaveChanges();
            return RedirectToAction("CancelIndex");
        }
        public async Task<IActionResult> Finished(int page = 1)
        {
            AppUser member = null;
            if (User.Identity.IsAuthenticated)
            {
                member = await _userManager.FindByNameAsync(User.Identity.Name);
            }
            var query = _appDbContext.Orders.Include(x => x.OrderItem).Where(x => x.AppUserId == member.Id).Where(x => x.IsCancel == false).Where(x => x.OrderItem.EndRentDate.DayOfYear < DateTime.Now.DayOfYear).AsQueryable();
            PaginatedList<Order> orders = PaginatedList<Order>.Create(query, 5, page);
            return View(orders);
        }
    }

}