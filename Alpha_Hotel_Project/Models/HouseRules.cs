using Alpha_Hotel_Project.Entities;
using System.ComponentModel.DataAnnotations;

namespace Alpha_Hotel_Project.Models
{
    public class HouseRules : EntityModel
    {
        [StringLength(maximumLength:90)]
        public string Name { get; set; }
    }
}
