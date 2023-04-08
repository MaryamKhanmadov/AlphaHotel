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
    public class FacilityController : Controller
    {
        private readonly AppDbContext _appDbContext;
        public FacilityController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public IActionResult Index(int page=1)
        {
            var query = _appDbContext.Facilities.Where(x=>x.IsDeleted==false).AsQueryable();
            PaginatedList<Facility> facilities = PaginatedList<Facility>.Create(query, 5, page);
            return View(facilities);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Facility facility)
        {
            if (!ModelState.IsValid) return View();
            _appDbContext.Facilities.Add(facility);
            _appDbContext.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult Update(Guid id)
        {
            Facility facility = _appDbContext.Facilities.FirstOrDefault(x => x.Id == id);
            if (facility == null) return View("Error");
            return View(facility);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(Facility facility)
        {
            if (!ModelState.IsValid) return View();
            Facility existfacility = _appDbContext.Facilities.FirstOrDefault(x => x.Id == facility.Id);
            if (existfacility == null) return View("Error");

            existfacility.Title = facility.Title;
            existfacility.Descreption = facility.Descreption;
            existfacility.Icon = facility.Icon;

            _appDbContext.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult Delete(Guid id)
        {
            Facility facility = _appDbContext.Facilities.FirstOrDefault(x => x.Id == id);
            if (facility == null) return View("Error");
            facility.IsDeleted = true;
            _appDbContext.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult SoftDeleteIndex(int page = 1)
        {
            var query = _appDbContext.Facilities.Where(x => x.IsDeleted == true).AsQueryable();
            PaginatedList<Facility> facilities = PaginatedList<Facility>.Create(query, 5, page);
            return View(facilities);
        }
        public IActionResult HardDelete(Guid id)
        {
            Facility facility = _appDbContext.Facilities.FirstOrDefault(x => x.Id == id);
            if (facility == null) return View("Error");
            _appDbContext.Facilities.Remove(facility);
            _appDbContext.SaveChanges();
            return RedirectToAction("SoftDeleteIndex");
        }
        public IActionResult Restore(Guid id)
        {
            Facility facility = _appDbContext.Facilities.FirstOrDefault(x => x.Id == id);
            if (facility == null) return View("Error");
            facility.IsDeleted = false;
            _appDbContext.SaveChanges();
            return RedirectToAction("SoftDeleteIndex");
        }
    }
}
