using NetBootcamp.API.DTOs;
using NetBootcamp.API.Roles.DTOs;
using NetBootcamp.API.Users;

namespace NetBootcamp.API.Roles
{
    public interface IRoleService
    {
        ResponseModelDto<IReadOnlyList<Role>> GetAll();
        ResponseModelDto<RoleDto?> GetById(int id);
        ResponseModelDto<RoleDto?> GetByName(string name);
        ResponseModelDto<int> Create(RoleCreateRequestDto request);
        ResponseModelDto<NoContent> Update(int roleId, RoleUpdateRequestDto request);
        ResponseModelDto<NoContent> Delete(int roleId);
    }
}
