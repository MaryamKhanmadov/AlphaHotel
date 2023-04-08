using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Alpha_Hotel_Project.Models
{
    public class Setting
    {
        public int Id { get; set; }
        [StringLength(maximumLength:30)]
        public string? Key { get; set; }
        [StringLength(maximumLength: 100)]
        public string? Value { get; set; }
        [NotMapped]
        public IFormFile? ImageFile { get; set; }
    }
}
