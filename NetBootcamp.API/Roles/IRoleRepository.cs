using NetBootcamp.API.Users;

namespace NetBootcamp.API.Roles
{
    public interface IRoleRepository
    {
        List<Role> GetAll();
        Role? GetById(int roleId);
        void Create(Role role);
        void Update(Role role);
        void Delete(int roleId);
    }
}
