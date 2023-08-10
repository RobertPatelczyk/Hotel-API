using HotelReview.Entities;

namespace HotelReview.Models.HotelRooms
{
    public class CreateHotelRoomsDto
    {
        public bool IsAvailable { get; set; }
        public int HowManyBeds { get; set; }
        public int Price { get; set; }
        public int HotelId { get; set; }
    }
}
