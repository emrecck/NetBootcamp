using NetBootcamp.Repository.Repositories;

namespace NetBootcamp.Repository.Tokens;

public class RefreshToken : BaseEntity<int>
{
    public Guid Code { get; set; }
    public DateTime Expire { get; set; }
    public Guid UserId { get; set; }
}
