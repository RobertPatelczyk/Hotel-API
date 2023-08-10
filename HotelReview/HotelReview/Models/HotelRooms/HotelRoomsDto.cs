using HotelReview.Models.Hotels;
using System.ComponentModel.DataAnnotations;

namespace HotelReview.Models.HotelRooms
{
    public class HotelRoomsDto
    {
        public int Id { get; set; }
        public bool IsAvailable { get; set; }
        public int HowManyBeds { get; set; }
        public int Price { get; set; }
    }
}
