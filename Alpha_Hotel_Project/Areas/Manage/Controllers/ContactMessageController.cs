using Alpha_Hotel_Project.Areas.Manage.ViewModels;
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
    public class ContactMessageController : Controller
    {
        private readonly AppDbContext _appDbContext;
        public ContactMessageController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public IActionResult Index(int page = 1)
        {
            var query = _appDbContext.ContactMessages.AsQueryable();
            PaginatedList<ContactMessage> messages = PaginatedList<ContactMessage>.Create(query, 10, page);
            DashboardViewModel dashboardViewModel = new DashboardViewModel
            {
                ContactMessagesPaginated = messages,
            };
            return View(dashboardViewModel);
        }
        public IActionResult Message(Guid id)
        {
            ContactMessage contactMessage = _appDbContext.ContactMessages.FirstOrDefault(x => x.Id == id);
            return View(contactMessage);
        }
    }
}