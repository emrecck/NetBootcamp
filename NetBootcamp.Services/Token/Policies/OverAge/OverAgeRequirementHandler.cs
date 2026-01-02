using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace NetBootcamp.Services.Token.Policies.OverAge;

public class OverAgeRequirementHandler : AuthorizationHandler<OverAgeRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, OverAgeRequirement requirement)
    {
        if (context.User.HasClaim(c => c.Type == ClaimTypes.DateOfBirth))
        {
            var dateOfBirthClaimValue = context.User.FindFirst(c => c.Type == ClaimTypes.DateOfBirth)?.Value;
            if (DateTime.TryParse(dateOfBirthClaimValue, out DateTime dateOfBirth))
            {
                var age = DateTime.Today.Year - dateOfBirth.Year;
                if (age >= requirement.Age)
                {
                    context.Succeed(requirement);
                }
            }
        }
        return Task.CompletedTask;
    }
}
