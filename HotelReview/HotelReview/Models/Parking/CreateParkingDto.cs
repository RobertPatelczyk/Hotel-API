using HotelReview.Models.ParkingPlace;
using System.Collections.Generic;

namespace HotelReview.Models.Parking
{
    public class CreateParkingDto
    {
        public bool AnyPlaces { get; set; }
        public int HotelId { get; set; }

        public List<ParkingPlaceDto> parkingPlaces { get; set; }

    }
}
