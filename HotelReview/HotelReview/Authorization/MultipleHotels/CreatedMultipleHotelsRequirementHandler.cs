using HotelReview.Entities;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace HotelReview.Authorization.MultipleHotels
{
    public class CreatedMultipleHotelsRequirementHandler : AuthorizationHandler<CreatedMultipleHotelsRequirement>
    {
        public HotelDbContext dbContext { get; }

        public CreatedMultipleHotelsRequirementHandler(HotelDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, CreatedMultipleHotelsRequirement requirement)
        {
            var userId = int.Parse(context.User.FindFirst(x => x.Type == ClaimTypes.NameIdentifier).Value);
            var createdRestaurantsCount = dbContext.Hotels.Count(x => x.CreatedById == userId);

            if (createdRestaurantsCount >= requirement.MinimumRestaurantCreated)
                context.Succeed(requirement);
            return Task.CompletedTask;
        }
    }
}
