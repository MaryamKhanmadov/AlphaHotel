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
    public class ProfessionController : Controller
    {
        private readonly AppDbContext _appDbContext;
        public ProfessionController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public IActionResult Index(int page=1)
        {
            var query = _appDbContext.Professions.Where(x=>x.IsDeleted==false).AsQueryable();
            PaginatedList<Profession> professions = PaginatedList<Profession>.Create(query, 5, page);
            return View(professions);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Profession profession)
        {
            if(!ModelState.IsValid) return View();
            _appDbContext.Professions.Add(profession);
            _appDbContext.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult Update(Guid id)
        {
            Profession profession = _appDbContext.Professions.FirstOrDefault(x => x.Id == id);
            if(profession == null) return View("Error");
            return View(profession);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(Profession profession)
        {
            if (!ModelState.IsValid) return View();
            Profession existprofession = _appDbContext.Professions.FirstOrDefault(x => x.Id == profession.Id);
            if(existprofession == null) return View("Error");

            existprofession.Name = profession.Name;

            _appDbContext.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult Delete(Guid id)
        {
            Profession profession = _appDbContext.Professions.FirstOrDefault(x => x.Id == id);
            if (profession == null) return View("Error");
            profession.IsDeleted = true;
            _appDbContext.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult SoftDeleteIndex(int page = 1)
        {
            var query = _appDbContext.Professions.Where(x => x.IsDeleted == true).AsQueryable();
            PaginatedList<Profession> professions = PaginatedList<Profession>.Create(query, 5, page);
            return View(professions);
        }
        public IActionResult HardDelete(Guid id)
        {
            Profession profession = _appDbContext.Professions.FirstOrDefault(x => x.Id == id);
            if (profession == null) return View("Error");
            _appDbContext.Professions.Remove(profession);
            _appDbContext.SaveChanges();
            return RedirectToAction("SoftDeleteIndex");
        }
        public IActionResult Restore(Guid id)
        {
            Profession profession = _appDbContext.Professions.FirstOrDefault(x => x.Id == id);
            if (profession == null) return View("Error");
            profession.IsDeleted = false;
            _appDbContext.SaveChanges();
            return RedirectToAction("SoftDeleteIndex");
        }
    }
}
