using System.ComponentModel.DataAnnotations;

namespace HotelReview.Entities
{
    public class Address
    {

        public int Id { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string PostalCode { get; set; }

        public Hotel Hotel { get; set; }
        public int HotelId { get; set; }
    }
}
