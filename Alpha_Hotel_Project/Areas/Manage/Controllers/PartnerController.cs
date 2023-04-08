using Alpha_Hotel_Project.Data;
using Alpha_Hotel_Project.Helpers;
using Alpha_Hotel_Project.Models;
using Alpha_Hotel_Project.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;
using System.Reflection.Metadata;

namespace Alpha_Hotel_Project.Areas.Manage.Controllers
{
    [Area("Manage")]
    [Authorize(Roles = "SuperAdmin,Admin")]
    public class PartnerController : Controller
    {
        private readonly AppDbContext _appDbContext;
        private readonly IWebHostEnvironment _env;
        public PartnerController(AppDbContext appDbContext, IWebHostEnvironment env)
        {
            _appDbContext = appDbContext;
            _env = env;
        }
        public IActionResult Index(int page=1)
        {
            var query = _appDbContext.Partners.Where(x => x.IsDeleted == false).AsQueryable();
            PaginatedList<Partner> partners = PaginatedList<Partner>.Create(query, 5, page);
            return View(partners);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Partner partner)
        {
            if (!ModelState.IsValid) return View();

            if (partner.ImageFile.ContentType != "image/png" && partner.ImageFile.ContentType != "image/jpeg")
            {
                ModelState.AddModelError("ImageFile", "You can only upload image type png or jpeg");
                return View();
            }
            if (partner.ImageFile.Length > 2097152)
            {
                ModelState.AddModelError("ImageFile", "You can only upload image size than lower 2mb");
                return View();
            }

            partner.Image = partner.ImageFile.SaveFile(_env.WebRootPath, "uploads/partner");

            _appDbContext.Partners.Add(partner);
            _appDbContext.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult Update(Guid id)
        {
            Partner partner = _appDbContext.Partners.FirstOrDefault(x => x.Id == id);
            if (partner == null) return View("Error");
            return View(partner);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(Partner partner)
        {
            if (!ModelState.IsValid) return View();
            Partner existpartner = _appDbContext.Partners.FirstOrDefault(x => x.Id == partner.Id);
            if (existpartner == null) return View("Error");
            if (partner.ImageFile is not null)
            {
                if (partner.ImageFile.ContentType != "image/png" && partner.ImageFile.ContentType != "image/jpeg")
                {
                    ModelState.AddModelError("ImageFile", "You can only upload image type png or jpeg");
                    return View();
                }
                if (partner.ImageFile.Length > 2097152)
                {
                    ModelState.AddModelError("ImageFile", "You can only upload image size than lower 2mb");
                    return View();
                }
                string path = Path.Combine(_env.WebRootPath, "uploads/partner", existpartner.Image);
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
                existpartner.Image = partner.ImageFile.SaveFile(_env.WebRootPath, "uploads/partner");
            }
            _appDbContext.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult Delete(Guid id)
        {
            Partner partner = _appDbContext.Partners.FirstOrDefault(x => x.Id == id);
            if (partner == null) return View("Error");
            partner.IsDeleted = true;
            _appDbContext.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult SoftDeleteIndex(int page = 1)
        {
            var query = _appDbContext.Partners.Where(x=>x.IsDeleted==true).AsQueryable();
            PaginatedList<Partner> partners = PaginatedList<Partner>.Create(query, 5, page);
            return View(partners);
        }
        public IActionResult HardDelete(Guid id)
        {
            Partner partner = _appDbContext.Partners.FirstOrDefault(x => x.Id == id);
            if (partner == null) return View("Error");
            string path = Path.Combine(_env.WebRootPath, "uploads/partner", partner.Image);
            System.IO.File.Delete(path);
            _appDbContext.Partners.Remove(partner);
            _appDbContext.SaveChanges();
            return RedirectToAction("SoftDeleteIndex");
        }
        public IActionResult Restore(Guid id)
        {
            Partner partner = _appDbContext.Partners.FirstOrDefault(x => x.Id == id);
            if (partner == null) return View("Error");
            partner.IsDeleted = false;
            _appDbContext.SaveChanges();
            return RedirectToAction("SoftDeleteIndex");
        }
    }
}
