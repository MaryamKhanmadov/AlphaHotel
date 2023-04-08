using Alpha_Hotel_Project.Data;
using Alpha_Hotel_Project.Models;
using Alpha_Hotel_Project.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Alpha_Hotel_Project.Controllers
{
    public class AboutController : Controller
    {
        private readonly AppDbContext _appDbContext;

        public AboutController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public IActionResult Index()
        {
            AboutViewModel aboutViewModel = new AboutViewModel
            {
                About = _appDbContext.Abouts.Take(6).FirstOrDefault(),
                Facilities = _appDbContext.Facilities.ToList(),
                Staffs = _appDbContext.Staffs.ToList(),
                Partners = _appDbContext.Partners.ToList(),
            };
            return View(aboutViewModel);
        }
    }
}
