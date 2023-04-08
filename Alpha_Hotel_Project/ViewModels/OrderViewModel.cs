using Alpha_Hotel_Project.Enums;
using Alpha_Hotel_Project.Models;
using System.ComponentModel.DataAnnotations;

namespace Alpha_Hotel_Project.ViewModels
{
    public class OrderViewModel
    {
        public CheckOutViewModel? CheckOutVM { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }
        public string? Note { get; set; }
        public Room? Room { get; set; }
        public string Fullname { get; set; }
        [Required]
        [StringLength(maximumLength: 30), DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }
        [Required]
        [StringLength(maximumLength: 100), DataType(DataType.EmailAddress)]
        public string? eMail { get; set; }
        [Required]
        public byte AdultCount { get; set; }
        [Required]
        public byte ChildCount { get; set; }

        [Required]
        [StringLength(maximumLength: 100)]
        public string Address { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime StartRentDate { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime EndRentDate { get; set; }
        public string? AppUserId { get; set; }
        public Guid? RoomId { get; set; }
        public AppUser? AppUser { get; set; }
        public double AdultPrice { get; set; }
        public List<Room>? Rooms { get; set; }
        public List<Partner>? Partners { get; set; }
        public List<Room>? RecentRooms { get; set; }
        public List<Room>? PopularRooms { get; set; }
        public List<HouseRules>? HouseRules { get; set; }
        public List<Feature>? Features { get; set; }
        public List<RoomFeature>? RoomFeatures { get; set; }
    }
}
