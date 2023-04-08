using System.ComponentModel.DataAnnotations;

namespace Alpha_Hotel_Project.Areas.Manage.ViewModels
{
    public class AdminRegisterViewModel
    {
        [StringLength(maximumLength: 30)]
        public string Fullname { get; set; }
        [StringLength(maximumLength: 30)]
        public string Username { get; set; }
        [StringLength(maximumLength: 70), DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [StringLength(maximumLength: 20, MinimumLength = 8), DataType(DataType.Password)]
        public string Password { get; set; }
        [StringLength(maximumLength: 20, MinimumLength = 8), DataType(DataType.Password)]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
    }
}
