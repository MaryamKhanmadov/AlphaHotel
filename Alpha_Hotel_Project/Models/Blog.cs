using Alpha_Hotel_Project.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Alpha_Hotel_Project.Models
{
    public class Blog : EntityModel
    {
        [Required]
        [StringLength(maximumLength: 50)]
        public string Title { get; set; }
        [StringLength(maximumLength: 100)]
        public string? ImageUrl { get; set; }
        public int ViewCount { get; set; }
        [Required]
        [StringLength(maximumLength: 1500)]
        public string Descreption { get; set; }
        [Required]
        [StringLength(maximumLength: 50)]
        public string SubHeading { get; set; }
        [Required]
        [StringLength(maximumLength: 500)]
        public string Quote { get; set; }
        [Required]
        [StringLength(maximumLength: 500)]
        public string SubHeadingDescreption { get; set; }
        public DateTime CreateDate { get; set; } = DateTime.Now;
        public BlogCategory? BlogCategory { get; set; }
        public Guid BlogCategoryId { get; set; }
        [Required]
        [StringLength(maximumLength: 100)]
        public string FacebookUrl { get; set; }
        [Required]
        [StringLength(maximumLength: 100)]
        public string LinkedinUrl { get; set; }
        [Required]
        [StringLength(maximumLength: 100)]
        public string PinterestUrl { get; set; }
        [Required]
        [StringLength(maximumLength: 100)]
        public string TwitterUrl { get; set; }
        [Required]
        [StringLength(maximumLength: 100)]
        public string GoogleUrl { get; set; }
        public List<BlogComment>? BlogComments { get; set; }
        [NotMapped]
        public IFormFile? ImageFile { get; set; }
    }
}
