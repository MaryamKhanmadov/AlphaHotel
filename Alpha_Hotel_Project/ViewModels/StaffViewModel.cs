using Alpha_Hotel_Project.Helpers;
using Alpha_Hotel_Project.Models;

namespace Alpha_Hotel_Project.ViewModels
{
    public class StaffViewModel
    {
        public PaginatedList<Staff> Staffs { get; set; }
        public List<Partner> Partners { get; set; }
    }
}
