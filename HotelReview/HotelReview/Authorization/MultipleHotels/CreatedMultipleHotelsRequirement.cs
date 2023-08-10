using Microsoft.AspNetCore.Authorization;

namespace HotelReview.Authorization.MultipleHotels
{
    public class CreatedMultipleHotelsRequirement : IAuthorizationRequirement
    {
        public CreatedMultipleHotelsRequirement(int minimumRestaurantCreated)
        {
            MinimumRestaurantCreated = minimumRestaurantCreated;
        }
        public int MinimumRestaurantCreated { get; }
    }
}
