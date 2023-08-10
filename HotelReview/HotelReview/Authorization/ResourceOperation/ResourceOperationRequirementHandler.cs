using HotelReview.Entities;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Threading.Tasks;

namespace HotelReview.Authorization.ResourceOperation
{
    public class ResourceOperationRequirementHandler : AuthorizationHandler<ResourceOperationRequirement, Hotel>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
            ResourceOperationRequirement requirement, Hotel hotel)
        {
            if (requirement.ResourceOperation == ResourceOperation.Read ||
                requirement.ResourceOperation == ResourceOperation.Create)
            {
                context.Succeed(requirement);
            }
            var userId = int.Parse(context.User.FindFirst(x => x.Type == ClaimTypes.NameIdentifier).Value);
            if (hotel.CreatedById == userId)
                context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }
}
