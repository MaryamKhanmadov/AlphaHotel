using Alpha_Hotel_Project.Data;
using Alpha_Hotel_Project.Helpers;
using Alpha_Hotel_Project.Models;
using Alpha_Hotel_Project.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Alpha_Hotel_Project.Areas.Manage.Controllers
{
    [Area("Manage")]
    [Authorize(Roles = "SuperAdmin,Admin")]
    public class StaffController : Controller
    {
        private readonly AppDbContext _appDbContext;
        private readonly IWebHostEnvironment _env;
        public StaffController(AppDbContext appDbContext , IWebHostEnvironment env)
        {
            _appDbContext = appDbContext;
            _env = env;
        }
        public IActionResult Index(int page=1)
        {
            var query = _appDbContext.Staffs.Include(x => x.Profession).Where(x => x.IsDeleted == false).AsQueryable();
            PaginatedList<Staff> staffs = PaginatedList<Staff>.Create(query, 5, page);
            return View(staffs);
        }
        public IActionResult Create()
        {
            ViewBag.Profession = _appDbContext.Professions.Where(x=>x.IsDeleted==false).ToList();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Staff staff)
        {
            ViewBag.Profession = _appDbContext.Professions.Where(x => x.IsDeleted == false).ToList();
            if (!ModelState.IsValid) return View();

            if (staff.ImageFile.ContentType != "image/png" && staff.ImageFile.ContentType != "image/jpeg")
            {
                ModelState.AddModelError("ImageFile", "You can only upload image type png or jpeg");
                return View();
            }
            if (staff.ImageFile.Length > 2097152)
            {
                ModelState.AddModelError("ImageFile", "You can only upload image size than lower 2mb");
                return View();
            }

            staff.ImageUrl = staff.ImageFile.SaveFile(_env.WebRootPath, "uploads/staff");

            _appDbContext.Staffs.Add(staff);
            _appDbContext.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult Update(Guid id)
        {
            ViewBag.Profession = _appDbContext.Professions.Where(x => x.IsDeleted == false).ToList();
            Staff staff = _appDbContext.Staffs.FirstOrDefault(x => x.Id == id);
            if (staff == null) return View("Error");
            return View(staff);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(Staff staff)
        {
            ViewBag.Profession = _appDbContext.Professions.Where(x => x.IsDeleted == false).ToList();
            if (!ModelState.IsValid) return View();
            Staff existstaff = _appDbContext.Staffs.FirstOrDefault(x => x.Id == staff.Id);
            if (existstaff == null) return View("Error");
            if (staff.ImageFile is not null)
            {
                if (staff.ImageFile.ContentType != "image/png" && staff.ImageFile.ContentType != "image/jpeg")
                {
                    ModelState.AddModelError("ImageFile", "You can only upload image type png or jpeg");
                    return View();
                }
                if (staff.ImageFile.Length > 2097152)
                {
                    ModelState.AddModelError("ImageFile", "You can only upload image size than lower 2mb");
                    return View();
                }
                string path = Path.Combine(_env.WebRootPath, "uploads/staff", existstaff.ImageUrl);
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
                existstaff.ImageUrl = staff.ImageFile.SaveFile(_env.WebRootPath, "uploads/staff");
            }
            existstaff.Fullname = staff.Fullname;
            existstaff.TwitterUrl = staff.TwitterUrl;
            existstaff.FacebookUrl = staff.FacebookUrl;
            existstaff.GoogleUrl = staff.GoogleUrl;
            existstaff.PhoneNumber = staff.PhoneNumber;
            existstaff.ProfessionId = staff.ProfessionId;
            existstaff.Descreption = staff.Descreption;
            _appDbContext.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult Delete(Guid id)
        {
            Staff staff = _appDbContext.Staffs.FirstOrDefault(x => x.Id == id);
            if (staff == null) return View("Error");
            staff.IsDeleted = true;
            _appDbContext.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult SoftDeleteIndex(int page = 1)
        {
            var query = _appDbContext.Staffs.Include(x => x.Profession).Where(x => x.IsDeleted == true).AsQueryable();
            PaginatedList<Staff> staffs = PaginatedList<Staff>.Create(query, 5, page);
            return View(staffs);
        }
        public IActionResult HardDelete(Guid id)
        {
            Staff staff = _appDbContext.Staffs.FirstOrDefault(x => x.Id == id);
            if (staff == null) return View("Error");
            string path = Path.Combine(_env.WebRootPath, "uploads/staff", staff.ImageUrl);
            System.IO.File.Delete(path);
            _appDbContext.Staffs.Remove(staff);
            _appDbContext.SaveChanges();
            return RedirectToAction("SoftDeleteIndex");
        }
        public IActionResult Restore(Guid id)
        {
            Staff staff = _appDbContext.Staffs.FirstOrDefault(x => x.Id == id);
            if (staff == null) return View("Error");
            staff.IsDeleted = false;
            _appDbContext.SaveChanges();
            return RedirectToAction("SoftDeleteIndex");
        }
    }
}
