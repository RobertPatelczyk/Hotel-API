using System;

namespace HotelReview.Exceptions
{
    public class BadRequestException :Exception
    {
        public BadRequestException(string message) : base(message)
        {
            
        }
    }
}
