using Alpha_Hotel_Project.Helpers;
using Alpha_Hotel_Project.Models;

namespace Alpha_Hotel_Project.ViewModels
{
    public class BlogViewModel
    {
        public PaginatedList<Blog>? Blogs { get; set; }
        public Blog? Blog { get; set; }
        public List<BlogCategory>? BlogCategories { get; set; }
        public BlogComment? BlogComment { get; set; }
        public List<BlogComment>? RecentBlogComment { get; set; }
        public List<BlogComment>? BlogComments { get; set; }
        public List<Partner>? Partners { get; set; }
        public List<Blog>? RecentBlogs { get; set; }
    }
}
