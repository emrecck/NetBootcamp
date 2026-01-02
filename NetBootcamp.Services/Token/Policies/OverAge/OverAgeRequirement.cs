using Microsoft.AspNetCore.Authorization;

namespace NetBootcamp.Services.Token.Policies.OverAge;

public class OverAgeRequirement : IAuthorizationRequirement
{
    public int Age { get; set; }
}
