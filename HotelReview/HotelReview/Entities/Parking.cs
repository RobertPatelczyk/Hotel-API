using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HotelReview.Entities
{
    public class Parking
    {
        public int Id { get; set; }
        public bool AnyPlaces { get; set; }

        public int HotelId { get; set; }
        public Hotel Hotel { get; set; }


        public List<ParkingPlace> ParkingPlace { get; set; }
    }
}
