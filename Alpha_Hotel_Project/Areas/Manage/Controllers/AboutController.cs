using Alpha_Hotel_Project.Data;
using Alpha_Hotel_Project.Helpers;
using Alpha_Hotel_Project.Models;
using Alpha_Hotel_Project.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Alpha_Hotel_Project.Areas.Manage.Controllers
{
    [Area("Manage")]
    [Authorize(Roles ="SuperAdmin,Admin")]
    public class AboutController : Controller
    {
        private readonly AppDbContext _appDbContext;
        private readonly IWebHostEnvironment _env;
        public AboutController(AppDbContext appDbContext, IWebHostEnvironment env)
        {
            _appDbContext = appDbContext;
            _env = env;
        }
        public IActionResult Index()
        {
            About about = _appDbContext.Abouts.Where(x => x.IsDeleted == false).FirstOrDefault();
            return View(about);
        }
        public IActionResult Update(Guid id)
        {
            About about = _appDbContext.Abouts.FirstOrDefault(x => x.Id == id);
            if (about == null) return View("Error");
            return View(about);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(About about)
        {
            if (!ModelState.IsValid) return View();
            About existabout = _appDbContext.Abouts.FirstOrDefault(x => x.Id == about.Id);
            if (existabout == null) return View("Error");

            if (about.ImageSmallFile is not null)
            {
                if (about.ImageSmallFile.ContentType != "image/png" && about.ImageSmallFile.ContentType != "image/jpeg")
                {
                    ModelState.AddModelError("ImageFile", "You can only upload image type png or jpeg");
                    return View();
                }
                if (about.ImageSmallFile.Length > 2097152)
                {
                    ModelState.AddModelError("ImageFile", "You can only upload image size than lower 2mb");
                    return View();
                }
                existabout.ImageSmall = about.ImageSmallFile.SaveFile(_env.WebRootPath, "uploads/about");
            }

            if (about.ImageBigFile is not null)
            {
                if (about.ImageBigFile.ContentType != "image/png" && about.ImageBigFile.ContentType != "image/jpeg")
                {
                    ModelState.AddModelError("ImageFile", "You can only upload image type png or jpeg");
                    return View();
                }
                if (about.ImageBigFile.Length > 2097152)
                {
                    ModelState.AddModelError("ImageFile", "You can only upload image size than lower 2mb");
                    return View();
                }
                existabout.ImageBig = about.ImageBigFile.SaveFile(_env.WebRootPath, "uploads/about");
            }

            existabout.ButtonText = about.ButtonText;
            existabout.Title = about.Title;
            existabout.ExperienceBoxText = about.ExperienceBoxText;

            _appDbContext.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
