using NetBootcamp.API.DTOs;
using NetBootcamp.API.Roles.DTOs;
using NetBootcamp.API.Users;
using System.Net;

namespace NetBootcamp.API.Roles
{
    public class RoleService(IRoleRepository roleRepository) : IRoleService
    {
        private readonly IRoleRepository _roleRepository = roleRepository;

        public ResponseModelDto<IReadOnlyList<Role>> GetAll()
        {
            return ResponseModelDto<IReadOnlyList<Role>>.Success(_roleRepository.GetAll());
        }

        public ResponseModelDto<RoleDto?> GetById(int id)
        {
            var role = _roleRepository.GetById(id);
            if (role is null)
                return ResponseModelDto<RoleDto?>.Fail("Rol bulunamadı !", HttpStatusCode.NotFound);

            var roleDto = new RoleDto(role.Id, role.Name);
            return ResponseModelDto<RoleDto?>.Success(roleDto);
        }

        public ResponseModelDto<RoleDto?> GetByName(string name)
        {
            var role = _roleRepository.GetByName(name);
            if (role is null)
                return ResponseModelDto<RoleDto?>.Fail("Rol bulunamadı !", HttpStatusCode.NotFound);

            var roleDto = new RoleDto(role.Id, role.Name);
            return ResponseModelDto<RoleDto?>.Success(roleDto);
        }

        public ResponseModelDto<int> Create(RoleCreateRequestDto request)
        {
            var isExist = _roleRepository.GetByName(request.Name);
            if (isExist is not null)
                return ResponseModelDto<int>.Fail("Rol zaten mevcut");    // default returns bad request status code 

            var addedRole = new Role
            {
                Id = _roleRepository.GetAll().Count() + 1,
                Name = request.Name,
            };
            _roleRepository.Create(addedRole);
            return ResponseModelDto<int>.Success(addedRole.Id, HttpStatusCode.Created);
        }

        public ResponseModelDto<NoContent> Update(int roleId, RoleUpdateRequestDto request)
        {
            var isExist = _roleRepository.GetById(roleId);
            if (isExist is null)
                return ResponseModelDto<NoContent>.Fail("Güncellemek istediğiniz rol bulunamadı !", HttpStatusCode.NotFound);

            var updatedRole = new Role
            {
                Id = roleId,
                Name = request.Name,
            };
            _roleRepository.Update(updatedRole);
            return ResponseModelDto<NoContent>.Success(HttpStatusCode.NoContent);
        }

        public ResponseModelDto<NoContent> Delete(int roleId)
        {
            var isExist = _roleRepository.GetById(roleId);
            if (isExist is null)
                return ResponseModelDto<NoContent>.Fail("Silmek istediğiniz kullanıcı mevcut değil !", HttpStatusCode.NotFound);

            _roleRepository.Delete(roleId);
            return ResponseModelDto<NoContent>.Success(HttpStatusCode.NoContent);
        }
    }
}
