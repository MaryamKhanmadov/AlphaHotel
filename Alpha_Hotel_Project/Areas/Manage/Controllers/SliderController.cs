using Alpha_Hotel_Project.Data;
using Alpha_Hotel_Project.Helpers;
using Alpha_Hotel_Project.Models;
using Alpha_Hotel_Project.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;

namespace Alpha_Hotel_Project.Areas.Manage.Controllers
{
    [Area("Manage")]
    [Authorize(Roles = "SuperAdmin,Admin")]
    public class SliderController : Controller
    {
        private readonly AppDbContext _appDbContext;
        private readonly IWebHostEnvironment _env;
        public SliderController(AppDbContext appDbContext, IWebHostEnvironment env)
        {
            _appDbContext = appDbContext;
            _env = env;
        }
        public IActionResult Index()
        {
            Slider slider = _appDbContext.Sliders.Where(x=>x.IsDeleted==false).FirstOrDefault();
            return View(slider);
        }
        public IActionResult Update(Guid id)
        {
            Slider slider = _appDbContext.Sliders.FirstOrDefault(x => x.Id == id);
            if (slider == null) return View("Error");
            return View(slider);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(Slider slider)
        {
            if (!ModelState.IsValid) return View();
            Slider existslider = _appDbContext.Sliders.FirstOrDefault(x => x.Id == slider.Id);
            if (existslider == null) return View("Error");
            if (slider.ImageFile is not null)
            {
                if (slider.ImageFile.ContentType != "image/png" && slider.ImageFile.ContentType != "image/jpeg")
                {
                    ModelState.AddModelError("ImageFile", "You can only upload image type png or jpeg");
                    return View();
                }
                if (slider.ImageFile.Length > 2097152)
                {
                    ModelState.AddModelError("ImageFile", "You can only upload image size than lower 2mb");
                    return View();
                }
                existslider.Image = slider.ImageFile.SaveFile(_env.WebRootPath, "uploads/slider");
            }
            existslider.Title = slider.Title;
            existslider.ButtonText = slider.ButtonText;
            existslider.Descreption = slider.Descreption;
            _appDbContext.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
