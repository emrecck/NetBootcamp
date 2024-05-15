
using NetBootcamp.API.Users;
using System.Xml.Linq;

namespace NetBootcamp.API.Roles
{
    public class RoleRepository : IRoleRepository
    {
        static readonly List<Role> _roleList =
        [
            new Role{ Id=1, Name="Admin"},
            new Role{ Id=2, Name="Editor"},
            new Role{ Id=3, Name="User"},
        ];

        public List<Role> GetAll()
        {
            return _roleList;
        }

        public Role? GetById(int roleId)
        {
            return _roleList.Find(x => x.Id == roleId);
        }

        public void Create(Role role)
        {
            _roleList.Add(role);
        }

        public void Update(Role role)
        {
            var index = _roleList.FindIndex(x => x.Id == role.Id);
            _roleList[index] = role;
        }

        public void Delete(int roleId)
        {
            var deletedItem = GetById(roleId);
            _roleList.Remove(deletedItem!);
        }
    }
}
