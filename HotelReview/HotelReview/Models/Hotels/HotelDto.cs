using HotelReview.Entities;
using HotelReview.Models.Address;
using HotelReview.Models.HotelRooms;
using HotelReview.Models.Parking;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HotelReview.Models.Hotels
{
    public class HotelDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool HasGym { get; set; }
        public int ReceptionNumber { get; set; }
        public string EmailContact { get; set; }


        public AddressDto Address { get; set; }
        public ParkingDto Parking { get; set; }
        public List<HotelRoomsDto> HotelRooms { get; set; }
    }
}
