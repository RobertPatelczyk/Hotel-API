using HotelReview.Entities;
using HotelReview.Models.Hotels;
using HotelReview.Models.ParkingPlace;
using System.Collections.Generic;

namespace HotelReview.Models.Parking
{
    public class ParkingDto
    {
        public int Id { get; set; }
        public bool AnyPlaces { get; set; }

        public List<ParkingPlaceDto> parkingPlaces { get; set; }
    }
}
