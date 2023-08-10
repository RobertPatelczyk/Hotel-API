namespace HotelReview.Models.Parking
{
    public class ParkingQuery
    {
        public int ParkingId { get; set; }
        public int FindParkingHotel { get; set; }
        public int hotelId { get; set; }
        public SampleQuery sample { get; set; }
    }
}
