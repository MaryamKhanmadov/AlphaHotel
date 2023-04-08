using Alpha_Hotel_Project.Entities;
using System.ComponentModel.DataAnnotations;

namespace Alpha_Hotel_Project.Models
{
    public class Feature : EntityModel
    {
        [StringLength(maximumLength: 20)]
        public string Name { get; set; }
        [StringLength(maximumLength: 100)]
        public string Icon { get; set; }
        public List<RoomFeature>? RoomFeatures { get; set; }
    }
}
