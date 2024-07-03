using Microsoft.EntityFrameworkCore;
using NetBootcamp.Repositories.Entities.Products;

namespace NetBootcamp.Repositories.Entities.Users
{
    public class UserRepository(EFDbContext context) : GenericRepository<User>(context), IUserRepository
    {
        public async Task<User> GetByPhoneNumber(string phoneNumber)
        {
            User? entity = await Entity.Where(x => x.PhoneNumber == phoneNumber).FirstOrDefaultAsync();
            return entity!;
        }
    }
}