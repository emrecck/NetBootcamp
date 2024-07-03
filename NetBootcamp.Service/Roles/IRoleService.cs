using NetBootcamp.Repositories.Entities.Roles;
using NetBootcamp.Service.Roles.DTOs;
using NetBootcamp.Service.Roles.RoleCreateUseCase;
using NetBootcamp.Service.SharedDTOs;
using System.Collections.Immutable;

namespace NetBootcamp.Service.Roles
{
    public interface IRoleService
    {
        Task<ResponseModelDto<IImmutableList<RoleDto>>> GetAll();
        Task<ResponseModelDto<RoleDto?>> GetById(int id);
        Task<ResponseModelDto<RoleDto?>> GetByName(string name);
        Task<ResponseModelDto<int>> Create(RoleCreateRequestDto request);
        Task<ResponseModelDto<NoContent>> Update(int roleId, RoleUpdateRequestDto request);
        Task<ResponseModelDto<NoContent>> Delete(int roleId);
    }
}
