using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelReview.Entities
{
    public class Hotel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool HasGym { get; set; }
        public int ReceptionNumber { get; set; }
        public string EmailContact { get; set; }

        public int? CreatedById { get; set; }
        public virtual User CreatedBy { get; set; }

        public  Address Address { get; set; }
        public  Parking Parking { get; set; }
        public  List<HotelRooms> Rooms { get; set; }
    }
}
