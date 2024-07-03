using AutoMapper;
using NetBootcamp.Core.CrossCuttingConcerns.Caching;
using NetBootcamp.Repositories;
using NetBootcamp.Repositories.Entities.Roles;
using NetBootcamp.Service.Roles.DTOs;
using NetBootcamp.Service.Roles.RoleCreateUseCase;
using NetBootcamp.Service.SharedDTOs;
using System.Collections.Immutable;
using System.Net;

namespace NetBootcamp.Service.Roles
{
    public class RoleService(IRoleRepository roleRepository, IUnitOfWork unitOfWork, IMapper mapper, CacheService cacheService) : IRoleService
    {
        //private readonly IRoleRepository roleRepository = roleRepository;

        public async Task<ResponseModelDto<IImmutableList<RoleDto>>> GetAll()
        {
            var roles = await roleRepository.GetAll();

            var roleDtos = mapper.Map<List<RoleDto>>(roles);

            return ResponseModelDto<IImmutableList<RoleDto>>.Success(roleDtos.ToImmutableList());
        }

        public async Task<ResponseModelDto<RoleDto?>> GetById(int id)
        {
            var role = await roleRepository.GetById(id);
            if (role is null)
                return ResponseModelDto<RoleDto?>.Fail("Rol bulunamadı !", HttpStatusCode.NotFound);

            var roleDto = mapper.Map<RoleDto>(role);
            return ResponseModelDto<RoleDto?>.Success(roleDto);
        }

        public async Task<ResponseModelDto<RoleDto?>> GetByName(string name)
        {
            var role = await roleRepository.GetByName(name);
            if (role is null)
                return ResponseModelDto<RoleDto?>.Fail("Rol bulunamadı !", HttpStatusCode.NotFound);

            var roleDto = mapper.Map<RoleDto>(role);
            return ResponseModelDto<RoleDto?>.Success(roleDto);
        }

        public async Task<ResponseModelDto<int>> Create(RoleCreateRequestDto request)
        {
            var isExist = await roleRepository.GetByName(request.Name);
            if (isExist is not null)
                return ResponseModelDto<int>.Fail("Rol zaten mevcut");    // default returns bad request status code 

            var addedRole = new Role
            {
                Name = request.Name,
                CreatedDate = DateTime.Now,
            };
            await roleRepository.Add(addedRole);
            await unitOfWork.CommitAsync();
            return ResponseModelDto<int>.Success(addedRole.Id, HttpStatusCode.Created);
        }

        public async Task<ResponseModelDto<NoContent>> Update(int roleId, RoleUpdateRequestDto request)
        {
            var isExist = await roleRepository.GetById(roleId);
            if (isExist is null)
                return ResponseModelDto<NoContent>.Fail("Güncellemek istediğiniz rol bulunamadı !", HttpStatusCode.NotFound);

            var updatedRole = new Role
            {
                Name = request.Name,
            };
            await roleRepository.Update(updatedRole);
            await unitOfWork.CommitAsync();
            return ResponseModelDto<NoContent>.Success(HttpStatusCode.NoContent);
        }

        public async Task<ResponseModelDto<NoContent>> Delete(int roleId)
        {
            var isExist = await roleRepository.GetById(roleId);
            if (isExist is null)
                return ResponseModelDto<NoContent>.Fail("Silmek istediğiniz kullanıcı mevcut değil !", HttpStatusCode.NotFound);

            await roleRepository.Delete(isExist);
            await unitOfWork.CommitAsync();
            return ResponseModelDto<NoContent>.Success(HttpStatusCode.NoContent);
        }
    }
}
