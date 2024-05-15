using NetBootcamp.API.DTOs;
using NetBootcamp.API.Users;

namespace NetBootcamp.API.Roles
{
    public interface IRoleService
    {
        ResponseModelDto<List<Role>> GetAll();
        ResponseModelDto<Role?> GetById(int id);
        ResponseModelDto<int> Create(Role request);
        ResponseModelDto<NoContent> Update(int roleId, Role request);
        ResponseModelDto<NoContent> Delete(int roleId);
    }
}
