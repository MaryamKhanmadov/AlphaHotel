using Alpha_Hotel_Project.Models;

namespace Alpha_Hotel_Project.ViewModels
{
    public class HomeViewModel
    {
        public List<Staff> Staffs { get; set; }
        public List<Profession> Professions { get; set; }
        public List<Facility> Facilities { get; set; }
        public List<Slider> Sliders { get; set; }
        public List<Partner> Partners { get; set; }
        public List<Room> Rooms { get; set; }
        public List<RoomImage> RoomImages { get; set; }
        public List<Category> Categories { get; set; }
        public List<Setting> Settings { get; set; }
        public List<Blog> Blogs { get; set; }
        public List<Blog> PopularBlogs { get; set; }
        public List<BlogComment> BlogComments { get; set; }
        public List<BlogCategory> BlogCategories { get; set; }
        public List<RoomFeature> RoomFeatures { get; set; }
        public List<Feature> Features { get; set; }
    }
}
