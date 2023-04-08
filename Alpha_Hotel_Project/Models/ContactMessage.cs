using Alpha_Hotel_Project.Entities;
using System.ComponentModel.DataAnnotations;

namespace Alpha_Hotel_Project.Models
{
    public class ContactMessage : EntityModel
    {
        [StringLength(maximumLength: 30)]
        public string FullName { get; set; }
        [StringLength(maximumLength: 200), DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [StringLength(maximumLength: 20)]
        public string Subject { get; set; }
        [StringLength(maximumLength: 20), DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }
        [StringLength(maximumLength: 200)]
        public string Message { get; set; }
        public DateTime MessageTime { get; set; } = DateTime.Now;
    }
}
