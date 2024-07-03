using System.Collections.Immutable;
using NetBootcamp.Service.SharedDTOs;
using NetBootcamp.Service.Users.DTOs;
using NetBootcamp.Service.Users.UserCreateUseCase;

namespace NetBootcamp.Service.Users
{
    public interface IUserService
    {
        Task<ResponseModelDto<IImmutableList<UserDto>>> GetAll();
        Task<ResponseModelDto<UserDto?>> GetById(int id);
        Task<ResponseModelDto<UserDto?>> GetByPhoneNumber(string phoneNumber);
        Task<ResponseModelDto<int>> Create(UserCreateRequestDto request);
        Task<ResponseModelDto<NoContent>> Update(int userId, UserUpdateRequestDto request);
        Task<ResponseModelDto<NoContent>> Delete(int userId);
    }
}
