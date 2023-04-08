using Alpha_Hotel_Project.Entities;
using System.ComponentModel.DataAnnotations;

namespace Alpha_Hotel_Project.Models
{
    public class Profession : EntityModel
    {
        [StringLength(maximumLength:50)]
        public string Name { get; set; }
        public List<Staff>? Staffs { get; set; }
    }
}
