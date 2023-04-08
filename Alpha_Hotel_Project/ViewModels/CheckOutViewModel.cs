using Alpha_Hotel_Project.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Alpha_Hotel_Project.ViewModels
{
    public class CheckOutViewModel
    {
        public OrderItem OrderItem { get; set; }
        public Room Room { get; set; }
        public Guid? OrderItemId { get; set; }
        public string? CardNumber { get; set; }
        public int? CardMonth { get; set; }
        public int? CardYear { get; set; }
        public int? CVV { get; set; }
        public List<HouseRules>? HouseRules { get; set; }
        public List<RoomFeature>? RoomFeatures { get; set; }
        public List<Feature>? Features { get; set; }
    }
}
