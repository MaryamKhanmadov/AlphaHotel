using Alpha_Hotel_Project.Entities;
using Alpha_Hotel_Project.Enums;
using System.ComponentModel.DataAnnotations;

namespace Alpha_Hotel_Project.Models
{
    public class Order : EntityModel
    {
        public OrderStatus OrderStatus { get; set; } = OrderStatus.Pending;
        [DataType(DataType.DateTime)]
        public DateTime CreateDate { get; set; } = DateTime.Now;
        public string? AppUserId { get; set; }
        public AppUser? AppUser { get; set; }
        public OrderItem OrderItem { get; set; }
        public Guid? RoomId { get; set; }
        [StringLength(maximumLength:16)]
        public string CardNumber { get; set; }
        public int CardMonth { get; set; }
        public int CardYear { get; set; }
        public int CVV { get; set; }
        public bool IsCancel { get; set; }
    }
}
