using Alpha_Hotel_Project.Data;
using Alpha_Hotel_Project.Helpers;
using Alpha_Hotel_Project.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NuGet.Configuration;
using System.Data;

namespace Alpha_Hotel_Project.Areas.Manage.Controllers
{
    [Area("Manage")]
    [Authorize(Roles = "SuperAdmin,Admin")]
    public class SettingController : Controller
    {
        private readonly AppDbContext _appDbContext;
        private readonly IWebHostEnvironment _env;
        public SettingController(AppDbContext appDbContext, IWebHostEnvironment env)
        {
            _appDbContext = appDbContext;
            _env = env;
        }
        public IActionResult Index(int page=1)
        {
            var query = _appDbContext.Settings.AsQueryable();
            PaginatedList<Setting> settings = PaginatedList<Setting>.Create(query, 5, page);
            return View(settings);
        }
        public IActionResult Update(int id)
        {
            Setting setting = _appDbContext.Settings.FirstOrDefault(x => x.Id == id);
            if (setting == null) { return NotFound(); }
            return View(setting);
        }
        [HttpPost]
        public IActionResult Update(Setting setting)
        {
            if (!ModelState.IsValid) { return View(setting); }
            Setting exsetting = _appDbContext.Settings.FirstOrDefault(x => x.Id == setting.Id);
            if (exsetting == null) return View("Error");
            if (setting.ImageFile is not null)
            {
                if (setting.ImageFile.ContentType != "image/png" && setting.ImageFile.ContentType != "image/jpeg")
                {
                    ModelState.AddModelError("ImageFile", "You can only upload image type png or jpeg");
                    return View();
                }
                if (setting.ImageFile.Length > 2097152)
                {
                    ModelState.AddModelError("ImageFile", "You can only upload image size than lower 2mb");
                    return View();
                }
                exsetting.Value = setting.ImageFile.SaveFileSetting(_env.WebRootPath, "assets/img/logos");
            }
            exsetting.Value = setting.Value;
            _appDbContext.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
