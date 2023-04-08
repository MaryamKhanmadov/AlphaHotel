using Alpha_Hotel_Project.Helpers;
using Alpha_Hotel_Project.Models;

namespace Alpha_Hotel_Project.ViewModels
{
    public class RoomViewModel
    {
        public PaginatedList<Room>? Rooms { get; set; }
        public List<Partner> Partners { get; set; }
        public List<Feature> Features { get; set; }
        public List<RoomFeature> RoomFeatures { get; set; }
    }
}
