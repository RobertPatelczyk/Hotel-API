using System.ComponentModel.DataAnnotations;

namespace HotelReview.Entities
{
    public class HotelRooms
    {
        public int Id { get; set; }


        public bool IsAvailable { get; set; }
        public int HowManyBeds { get; set; }
        public int Price { get; set; }

        public int HotelId { get; set; }
        public Hotel Hotel { get; set; }
    }
}
