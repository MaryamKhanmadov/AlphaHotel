using Alpha_Hotel_Project.Entities;
using System.ComponentModel.DataAnnotations;

namespace Alpha_Hotel_Project.Models
{
    public class Facility : EntityModel
    {
        [StringLength(maximumLength:30)]
        public string Title { get; set; }
        [StringLength(maximumLength: 100)]
        public string Descreption { get; set; }
        [StringLength(maximumLength: 100)]
        public string Icon { get; set; }
    }
}
