using Alpha_Hotel_Project.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Alpha_Hotel_Project.Models
{
    public class Slider : EntityModel
    {
        [StringLength(maximumLength:30)]
        public string Title { get; set; }
        [StringLength(maximumLength: 130)]
        public string Descreption { get; set; }
        [StringLength(maximumLength: 150)]
        public string? Image { get; set; }
        [StringLength(maximumLength: 50)]
        public string ButtonText { get; set; }
        [NotMapped]
        public IFormFile? ImageFile { get; set; }
    }
}
