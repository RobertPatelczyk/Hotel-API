namespace HotelReview.Models.ParkingPlace
{
    public class CreateParkingPlaceDto
    {
        public int Price { get; set; }
        public string Size { get; set; }
        public bool Camera { get; set; }
        public int ParkingId { get; set; }
    }
}
