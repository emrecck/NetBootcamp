using NetBootcamp.API.Roles.DTOs;
using NetBootcamp.API.Users;

namespace NetBootcamp.API.Roles
{
    public interface IRoleRepository
    {
        IReadOnlyList<Role> GetAll();
        Role? GetById(int roleId);
        Role? GetByName(string name);
        void Create(Role role);
        void Update(Role role);
        void Delete(int roleId);
    }
}
