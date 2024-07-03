using AutoMapper;
using Microsoft.Extensions.Caching.Memory;
using NetBootcamp.Core.CrossCuttingConcerns.Caching;
using NetBootcamp.Repositories;
using NetBootcamp.Repositories.Entities.Users;
using NetBootcamp.Service.SharedDTOs;
using NetBootcamp.Service.Users.DTOs;
using NetBootcamp.Service.Users.UserCreateUseCase;
using System.Collections.Immutable;
using System.Net;
using System.Text.Json;

namespace NetBootcamp.Service.Users
{
    public class UserService(IUserRepository userRepository, IUnitOfWork unitOfWork, IMapper mapper, CacheService cacheService) : IUserService
    {
        public async Task<ResponseModelDto<IImmutableList<UserDto>>> GetAll()
        {
            IImmutableList<UserDto> data;
            var isExist = cacheService.IsExistCache(cacheService.CreateCacheKey("UserService", "GetAll", "UserDtos"), out data);
                
            if (isExist)
            {
                return ResponseModelDto<IImmutableList<UserDto>>.Success(data);
            }

            var list = await userRepository.GetAll();
            var response = mapper.Map<List<UserDto>>(list);

            cacheService.AddCache(cacheService.CreateCacheKey("UserService", "GetAll", "UserDtos"), response);

            return ResponseModelDto<IImmutableList<UserDto>>.Success(response.ToImmutableList());
        }

        public async Task<ResponseModelDto<UserDto?>> GetById(int id)
        {
            var user = await userRepository.GetById(id);
            if (user is null)
                return ResponseModelDto<UserDto?>.Fail("Kullanıcı bulunamadı !", HttpStatusCode.NotFound);

            var newUserDto = mapper.Map<UserDto>(user);
            return ResponseModelDto<UserDto?>.Success(newUserDto);
        }

        public async Task<ResponseModelDto<UserDto?>> GetByPhoneNumber(string phoneNumber)
        {
            var user = await userRepository.GetByPhoneNumber(phoneNumber);
            if (user is null)
                return ResponseModelDto<UserDto?>.Fail("Kullanıcı bulunamadı !", HttpStatusCode.NotFound);

            var newUserDto = mapper.Map<UserDto>(user);
            return ResponseModelDto<UserDto?>.Success(newUserDto);
        }

        public async Task<ResponseModelDto<int>> Create(UserCreateRequestDto request)
        {
            var isExist = await userRepository.GetByPhoneNumber(request.PhoneNumber);
            if (isExist is not null)
                return ResponseModelDto<int>.Fail("Kullanıcı zaten mevcut");    // default returns bad request status code 

            var newUser = new User
            {
                Name = request.Name,
                Surname = request.Surname,
                PhoneNumber = request.PhoneNumber,
                Email = request.Email,
                CreatedDate = DateTime.UtcNow
            };
            await userRepository.Add(newUser);
            await unitOfWork.CommitAsync();
            return ResponseModelDto<int>.Success(newUser.Id, HttpStatusCode.Created);
        }

        public async Task<ResponseModelDto<NoContent>> Update(int userId, UserUpdateRequestDto request)
        {
            var isExist = await userRepository.GetById(userId);
            if (isExist is null)
                return ResponseModelDto<NoContent>.Fail("Güncellemek istediğiniz kullanıcı bulunamadı !", HttpStatusCode.NotFound);

            var updatedUser = new User
            {
                Id = userId,
                Name = request.Name,
                Surname = request.Surname,
                PhoneNumber = request.PhoneNumber,
                Email = request.Email
            };

            await userRepository.Update(updatedUser);
            await unitOfWork.CommitAsync();
            return ResponseModelDto<NoContent>.Success(HttpStatusCode.NoContent);
        }

        public async Task<ResponseModelDto<NoContent>> Delete(int userId)
        {
            var isExist = await userRepository.GetById(userId);
            if (isExist is null)
                return ResponseModelDto<NoContent>.Fail("Silmek istediğiniz kullanıcı bulunamadı !", HttpStatusCode.NotFound);

            await userRepository.Delete(isExist);
            await unitOfWork.CommitAsync();
            return ResponseModelDto<NoContent>.Success(HttpStatusCode.NoContent);
        }
    }
}
