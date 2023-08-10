namespace HotelReview.Entities
{
    public class ParkingPlace
    {
        public int Id { get; set; }
        public int Price { get; set; }
        public string Size { get; set; }
        public bool Camera { get; set; }

        public Parking Parking { get; set; }
        public int ParkingId { get; set; }
    }
}
