using System.ComponentModel.DataAnnotations;

namespace HotelReview.Models.Hotels
{
    public class UpdateHotelDto
    {
        [Required]
        [Range(100000000, 999999999)]
        public int ReceptionNumber { get; set; }
        public bool HasGym { get; set; }
        public string EmailContact { get; set; }

        public HotelDto HotelDto { get; set; }
        public int HotelDtoId { get; set; }
    }
}
