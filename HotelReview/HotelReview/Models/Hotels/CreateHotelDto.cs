using HotelReview.Entities;
using HotelReview.Models.Address;
using HotelReview.Models.HotelRooms;
using HotelReview.Models.Parking;
using System.ComponentModel.DataAnnotations;

namespace HotelReview.Models.Hotels
{
    public class CreateHotelDto
    {
        [Required]
        [MaxLength(25)]
        public string Name { get; set; }
        public bool HasGym { get; set; }
        public int ReceptionNumber { get; set; }
        public string EmailContact { get; set; }

        public AddressDto Address { get; set; }
        public ParkingDto Parkings { get; set; }
        public HotelRoomsDto HotelRooms { get; set; }
    }
}
