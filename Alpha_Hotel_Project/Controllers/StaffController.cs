using Alpha_Hotel_Project.Data;
using Alpha_Hotel_Project.Helpers;
using Alpha_Hotel_Project.Models;
using Alpha_Hotel_Project.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Alpha_Hotel_Project.Controllers
{
    public class StaffController : Controller
    {
        private readonly AppDbContext _appDbContext;

        public StaffController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public IActionResult Index(int page=1)
        {
            var query = _appDbContext.Staffs.Include(x => x.Profession).Where(x => x.IsDeleted == false).AsQueryable();
            PaginatedList<Staff> staffs = PaginatedList<Staff>.Create(query, 4, page);
            StaffViewModel staffVM = new StaffViewModel
            {
                Staffs = staffs,
                Partners = _appDbContext.Partners.ToList()
            };
            return View(staffVM);
        }
    }
}
