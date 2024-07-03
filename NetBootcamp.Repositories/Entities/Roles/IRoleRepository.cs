namespace NetBootcamp.Repositories.Entities.Roles
{
    public interface IRoleRepository : IGenericRepository<Role>
    {
        Task<Role> GetByName(string name);
    }
}
