using Microsoft.EntityFrameworkCore;
using NetBootcamp.Repositories.Entities.Products;
using System.Collections.Immutable;

namespace NetBootcamp.Repositories.Entities.Roles
{
    public class RoleRepository(EFDbContext context) : GenericRepository<Role>(context), IRoleRepository
    {
        public async Task<Role> GetByName(string name)
        {
            var entity = await Entity.Where(x=>x.Name == name).FirstOrDefaultAsync();
            return entity!;
        }
    }
}
