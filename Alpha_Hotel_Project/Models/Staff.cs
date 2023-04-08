using Alpha_Hotel_Project.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Alpha_Hotel_Project.Models
{
    public class Staff : EntityModel
    {
        [StringLength(maximumLength: 50)]
        public string Fullname { get; set; }

        [StringLength(maximumLength: 250)]
        public string Descreption { get; set; }

        [StringLength(maximumLength: 150)]
        public string TwitterUrl { get; set; }
        
        [StringLength(maximumLength: 150)]
        public string GoogleUrl { get; set; }

        [StringLength(maximumLength: 150)]
        public string FacebookUrl { get; set; }

        [StringLength(maximumLength: 100)]
        public string? ImageUrl { get; set; }

        [StringLength(maximumLength: 30)]
        public string PhoneNumber { get; set; }
        [NotMapped]
        public IFormFile? ImageFile { get; set; }
        public Guid ProfessionId { get; set; }
        public Profession? Profession { get; set; }
    }
}
