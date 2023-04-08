using Alpha_Hotel_Project.Data;
using Alpha_Hotel_Project.Helpers;
using Alpha_Hotel_Project.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;

namespace Alpha_Hotel_Project.Areas.Manage.Controllers
{
    [Area("Manage")]
    [Authorize(Roles = "SuperAdmin,Admin")]
    public class BlogCategoryController : Controller
    {
        private readonly AppDbContext _appDbContext;

        public BlogCategoryController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public IActionResult Index(int page=1)
        {
            var query = _appDbContext.BlogCategories.Where(x => x.IsDeleted == false).AsQueryable();
            PaginatedList<BlogCategory> blogCategories = PaginatedList<BlogCategory>.Create(query, 5, page);
            return View(blogCategories);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(BlogCategory category)
        {
            if (!ModelState.IsValid) return View();
            _appDbContext.BlogCategories.Add(category);
            _appDbContext.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult Update(Guid id)
        {
            BlogCategory category = _appDbContext.BlogCategories.FirstOrDefault(x => x.Id == id);
            if (category == null) return View("Error");
            return View(category);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(BlogCategory category)
        {
            if (!ModelState.IsValid) return View();
            BlogCategory existcategory = _appDbContext.BlogCategories.FirstOrDefault(x => x.Id == category.Id);
            if (existcategory == null) return View("Error");

            existcategory.Name = category.Name;

            _appDbContext.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult Delete(Guid id)
        {
            BlogCategory category = _appDbContext.BlogCategories.FirstOrDefault(x => x.Id == id);
            if (category == null) return View("Error");
            category.IsDeleted = true;
            _appDbContext.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult SoftDeleteIndex(int page=1)
        {
            var query = _appDbContext.BlogCategories.Where(x => x.IsDeleted == true).AsQueryable();
            PaginatedList<BlogCategory> blogCategories = PaginatedList<BlogCategory>.Create(query, 5, page);
            return View(blogCategories);
        }
        public IActionResult HardDelete(Guid id)
        {
            BlogCategory category = _appDbContext.BlogCategories.FirstOrDefault(x => x.Id == id);
            if (category == null) return View("Error");
            _appDbContext.BlogCategories.Remove(category);
            _appDbContext.SaveChanges();
            return RedirectToAction("SoftDeleteIndex");
        }
        public IActionResult Restore(Guid id)
        {
            BlogCategory category = _appDbContext.BlogCategories.FirstOrDefault(x => x.Id == id);
            if (category == null) return View("Error");
            category.IsDeleted = false;
            _appDbContext.SaveChanges();
            return RedirectToAction("SoftDeleteIndex");
        }
    }
}
