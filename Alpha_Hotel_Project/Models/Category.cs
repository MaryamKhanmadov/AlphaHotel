using Alpha_Hotel_Project.Entities;
using System.ComponentModel.DataAnnotations;

namespace Alpha_Hotel_Project.Models
{
    public class Category : EntityModel
    {
        [StringLength(maximumLength:20)]
        public string Name { get; set; }
        public List<Room>? Rooms { get; set; }
    }
}
