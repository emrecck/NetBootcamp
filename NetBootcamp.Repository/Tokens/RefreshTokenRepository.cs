using NetBootcamp.Repository.Repositories;

namespace NetBootcamp.Repository.Tokens;

public class RefreshTokenRepository(AppDbContext context) : GenericRepository<RefreshToken>(context), IRefreshTokenRepository
{
}
