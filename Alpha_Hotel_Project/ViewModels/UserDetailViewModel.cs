using Alpha_Hotel_Project.Models;

namespace Alpha_Hotel_Project.ViewModels
{
    public class UserDetailViewModel
    {
        public AppUser User { get; set; }
        public IList<string> UserRoles { get; set; }
    }
}
