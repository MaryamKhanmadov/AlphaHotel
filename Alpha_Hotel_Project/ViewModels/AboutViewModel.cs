using Alpha_Hotel_Project.Models;

namespace Alpha_Hotel_Project.ViewModels
{
    public class AboutViewModel
    {
        public About About { get; set; }
        public List<Facility> Facilities { get; set; }
        public List<Staff> Staffs { get; set; }
        public List<Partner> Partners { get; set; }
    }
}
