using System.ComponentModel.DataAnnotations;

namespace HotelReview.Models.Address
{
    public class CreateAddressDto
    {
        [Required]
        [MaxLength(25)]
        public string City { get; set; }
        [Required]
        [MaxLength(50)]
        public string Street { get; set; }
        public string PostalCode { get; set; }

        public int HotelId { get; set; }
    }
}
