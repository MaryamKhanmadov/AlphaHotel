using Alpha_Hotel_Project.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Alpha_Hotel_Project.Models
{
    public class About : EntityModel
    {
        [StringLength(maximumLength: 70)]
        public string Title { get; set; }
        [StringLength(maximumLength: 30)]
        public string ExperienceBoxText { get; set; }

        [StringLength(maximumLength: 15)]
        public string ButtonText { get; set; }

        [StringLength(maximumLength: 100)]
        public string? ImageBig { get; set; }

        [StringLength(maximumLength: 100)]
        public string? ImageSmall { get; set; }
        [NotMapped]
        public IFormFile? ImageSmallFile { get; set; }
        [NotMapped]
        public IFormFile? ImageBigFile { get; set; }
    }
}
