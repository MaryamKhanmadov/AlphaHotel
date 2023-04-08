using Alpha_Hotel_Project.Entities;

namespace Alpha_Hotel_Project.Models
{
    public class RoomFeature : EntityModel
    {
        public Guid? FeatureId { get; set; }
        public Guid? RoomId { get; set; }
        public Room? Room { get; set; }
        public Feature? Feature { get; set; }
    }
}
