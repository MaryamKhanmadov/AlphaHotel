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
    public class FeatureController : Controller
    {
        private readonly AppDbContext _appDbContext;
        public FeatureController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public IActionResult Index(int page = 1)
        {
            var query = _appDbContext.Features.Where(x => x.IsDeleted == false).AsQueryable();
            PaginatedList<Feature> features = PaginatedList<Feature>.Create(query, 5, page);
            return View(features);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Feature feature)
        {
            if (!ModelState.IsValid) return View();
            _appDbContext.Features.Add(feature);
            _appDbContext.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult Update(Guid id)
        {
            Feature feature = _appDbContext.Features.FirstOrDefault(x => x.Id == id);
            if (feature == null) return View("Error");
            return View(feature);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(Feature feature)
        {
            if (!ModelState.IsValid) return View();
            Feature existfeature = _appDbContext.Features.FirstOrDefault(x => x.Id == feature.Id);
            if (existfeature == null) return View("Error");
            existfeature.Name = feature.Name;
            _appDbContext.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult Delete(Guid id)
        {
            Feature feature = _appDbContext.Features.FirstOrDefault(x => x.Id == id);
            if (feature == null) return View("Error");
            feature.IsDeleted = true;
            _appDbContext.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult SoftDeleteIndex(int page = 1)
        {
            var query = _appDbContext.Features.Where(x => x.IsDeleted == true).AsQueryable();
            PaginatedList<Feature> features = PaginatedList<Feature>.Create(query, 5, page);
            return View(features);
        }
        public IActionResult HardDelete(Guid id)
        {
            Feature feature = _appDbContext.Features.FirstOrDefault(x => x.Id == id);
            if (feature == null) return View("Error");
            _appDbContext.Features.Remove(feature);
            _appDbContext.SaveChanges();
            return RedirectToAction("SoftDeleteIndex");
        }
        public IActionResult Restore(Guid id)
        {
            Feature feature = _appDbContext.Features.FirstOrDefault(x => x.Id == id);
            if (feature == null) return View("Error");
            feature.IsDeleted = false;
            _appDbContext.SaveChanges();
            return RedirectToAction("SoftDeleteIndex");
        }
    }
}
