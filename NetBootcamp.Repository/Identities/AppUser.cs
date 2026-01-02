using Microsoft.AspNetCore.Identity;

namespace NetBootcamp.Repository.Identity;

public class AppUser : IdentityUser<Guid>
{
    public string Name { get; set; } = default!;
    public string Surname { get; set; } = default!;
    public DateTime? BirthDate { get; set; }
}
