using Alpha_Hotel_Project.Entities;
using System.ComponentModel.DataAnnotations;

namespace Alpha_Hotel_Project.Models
{
    public class RoomImage:EntityModel
    {
        public Guid? RoomId { get; set; }
        [StringLength(maximumLength: 100)]
        public string? ImageUrl { get; set; }
        public bool IsPoster { get; set; }
        public Room? Room { get; set; }
    }
}
