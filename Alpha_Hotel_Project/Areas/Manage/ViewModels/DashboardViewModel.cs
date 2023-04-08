using Alpha_Hotel_Project.Helpers;
using Alpha_Hotel_Project.Models;

namespace Alpha_Hotel_Project.Areas.Manage.ViewModels
{
    public class DashboardViewModel
    {
        public List<Order> Orders { get; set; }
        public List<OrderItem> OrderItems { get; set; }
        public List<Order> RecentOrders { get; set; }
        public List<AppUser> AppUsers { get; set; }
        public List<ContactMessage>? ContactMessages { get; set; }
        public PaginatedList<ContactMessage>? ContactMessagesPaginated { get; set; }
    }
}
