using Alpha_Hotel_Project.Data;
using Alpha_Hotel_Project.Models;
using Alpha_Hotel_Project.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol;
using System.Diagnostics;

namespace Alpha_Hotel_Project.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _appDbContext;

        public HomeController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public IActionResult Index()
        {
            HomeViewModel homeViewModel = new HomeViewModel
            {
                RoomFeatures = _appDbContext.RoomFeatures.ToList(),
                Features = _appDbContext.Features.Where(x=>x.IsDeleted==false).ToList(),
                Staffs = _appDbContext.Staffs.Include(x => x.Profession).Where(x => x.IsDeleted == false).ToList(),
                Facilities = _appDbContext.Facilities.Where(x => x.IsDeleted == false).ToList(),
                Sliders = _appDbContext.Sliders.Where(x => x.IsDeleted == false).ToList(),
                Partners = _appDbContext.Partners.Where(x => x.IsDeleted == false).ToList(),
                Rooms = _appDbContext.Rooms.Include(x=>x.RoomFeatures).Include(x => x.Category).Where(x => x.IsDeleted == false).OrderByDescending(x => x.ViewCount).Take(6).ToList(),
                RoomImages = _appDbContext.RoomImages.ToList(),
                Settings = _appDbContext.Settings.ToList(),
                Blogs = _appDbContext.Blogs.Include(x => x.BlogCategory).Include(x => x.BlogComments).Where(x => x.IsDeleted == false).OrderByDescending(x => x.CreateDate).Take(3).ToList(),
                PopularBlogs = _appDbContext.Blogs.Where(x => x.IsDeleted == false).OrderByDescending(x => x.ViewCount).Take(4).ToList(),
                BlogComments = _appDbContext.BlogComments.OrderByDescending(x => x.MessageTime).Take(3).ToList(),
            };
            return View(homeViewModel);
        }
    }
}