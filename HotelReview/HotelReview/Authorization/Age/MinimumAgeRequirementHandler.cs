﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace HotelReview.Authorization.Age
{
    public class MinimumAgeRequirementHandler : AuthorizationHandler<MinimumAgeRequirement>
    {
        private readonly ILogger<MinimumAgeRequirement> logger;

        public MinimumAgeRequirementHandler(ILogger<MinimumAgeRequirement> logger)
        {
            this.logger = logger;
        }
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, MinimumAgeRequirement requirement)
        {
            var dateOfBirth = DateTime.Parse(context.User.FindFirst(x => x.Type == "DateOfBirth").Value);
            var userEmail = context.User.FindFirst(x => x.Type == ClaimTypes.Name).Value;

            logger.LogInformation($"user: {userEmail} with date of birth:{dateOfBirth}");
            if (dateOfBirth.AddYears(requirement.MinimumAge) <= DateTime.Today)
            {
                logger.LogInformation("Authorization succedded");
                context.Succeed(requirement);
            }
            else
            {
                logger.LogInformation("Authorization failed");
            }
            return Task.CompletedTask;
        }
    }
}
