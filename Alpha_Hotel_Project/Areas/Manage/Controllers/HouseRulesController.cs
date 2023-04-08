using Alpha_Hotel_Project.Data;
using Alpha_Hotel_Project.Helpers;
using Alpha_Hotel_Project.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace Alpha_Hotel_Project.Areas.Manage.Controllers
{
    [Area("Manage")]
    [Authorize(Roles = "SuperAdmin,Admin")]
    public class HouseRulesController : Controller
    {
        private readonly AppDbContext _appDbContext;

        public HouseRulesController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public IActionResult Index(int page = 1)
        {
            var query = _appDbContext.HouseRules.Where(x => x.IsDeleted == false).AsQueryable();
            PaginatedList<HouseRules> houseRules = PaginatedList<HouseRules>.Create(query, 5, page);
            return View(houseRules);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(HouseRules houseRules)
        {
            if (!ModelState.IsValid) return View();
            _appDbContext.HouseRules.Add(houseRules);
            _appDbContext.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult Update(Guid id)
        {
            HouseRules houseRules = _appDbContext.HouseRules.FirstOrDefault(x => x.Id == id);
            if (houseRules == null) return View("Error");
            return View(houseRules);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(HouseRules houseRules)
        {
            if (!ModelState.IsValid) return View();
            HouseRules existhouseRules = _appDbContext.HouseRules.FirstOrDefault(x => x.Id == houseRules.Id);
            if (existhouseRules == null) return View("Error");

            existhouseRules.Name = houseRules.Name;

            _appDbContext.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult Delete(Guid id)
        {
            HouseRules houseRules = _appDbContext.HouseRules.FirstOrDefault(x => x.Id == id);
            if (houseRules == null) return View("Error");
            houseRules.IsDeleted = true;
            _appDbContext.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult SoftDeleteIndex(int page = 1)
        {
            var query = _appDbContext.HouseRules.Where(x => x.IsDeleted == true).AsQueryable();
            PaginatedList<HouseRules> houseRules = PaginatedList<HouseRules>.Create(query, 5, page);
            return View(houseRules);
        }
        public IActionResult HardDelete(Guid id)
        {
            HouseRules houseRules = _appDbContext.HouseRules.FirstOrDefault(x => x.Id == id);
            if (houseRules == null) return View("Error");
            _appDbContext.HouseRules.Remove(houseRules);
            _appDbContext.SaveChanges();
            return RedirectToAction("SoftDeleteIndex");
        }
        public IActionResult Restore(Guid id)
        {
            HouseRules houseRules = _appDbContext.HouseRules.FirstOrDefault(x => x.Id == id);
            if (houseRules == null) return View("Error");
            houseRules.IsDeleted = false;
            _appDbContext.SaveChanges();
            return RedirectToAction("SoftDeleteIndex");
        }
    }
}
