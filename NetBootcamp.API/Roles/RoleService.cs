using NetBootcamp.API.DTOs;
using NetBootcamp.API.Users;
using System.Net;

namespace NetBootcamp.API.Roles
{
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _roleRepository;
        public RoleService(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        public ResponseModelDto<List<Role>> GetAll()
        {
            return ResponseModelDto<List<Role>>.Success(_roleRepository.GetAll());
        }

        public ResponseModelDto<Role?> GetById(int id)
        {
            var role = _roleRepository.GetById(id);
            if (role is null)
                return ResponseModelDto<Role?>.Fail("Kullanıcı bulunamadı !", HttpStatusCode.NotFound);

            return ResponseModelDto<Role?>.Success(role);
        }

        public ResponseModelDto<int> Create(Role request)
        {
            var isExist = _roleRepository.GetById(request.Id);
            if (isExist is not null)
                return ResponseModelDto<int>.Fail("Kullanıcı zaten mevcut");    // default returns bad request status code 

            _roleRepository.Create(request);
            return ResponseModelDto<int>.Success(request.Id);
        }

        public ResponseModelDto<NoContent> Update(int roleId, Role request)
        {
            var isExist = _roleRepository.GetById(roleId);
            if (isExist is null)
                return ResponseModelDto<NoContent>.Fail("Güncellemek istediğiniz kullanıcı bulunamadı !", HttpStatusCode.NotFound);

            _roleRepository.Update(request);
            return ResponseModelDto<NoContent>.Success();
        }

        public ResponseModelDto<NoContent> Delete(int roleId)
        {
            var isExist = _roleRepository.GetById(roleId);
            if (isExist is null)
                return ResponseModelDto<NoContent>.Fail("Silmek istediğiniz kullanıcı mevcut değil !", HttpStatusCode.NotFound);

            _roleRepository.Delete(roleId);
            return ResponseModelDto<NoContent>.Success();
        }
    }
}
