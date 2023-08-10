using System;

namespace HotelReview.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string message):base(message)
        {

        }
    }
}
