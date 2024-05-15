using NetBootcamp.API.DTOs;
using NetBootcamp.API.Products.DTOs;
using NetBootcamp.API.Products;
using System.Collections.Immutable;
using NetBootcamp.API.Users.DTOs;

namespace NetBootcamp.API.Users
{
    public interface IUserService
    {
        ResponseModelDto<IReadOnlyList<UserDto>> GetAll();
        ResponseModelDto<UserDto?> GetById(int id);
        ResponseModelDto<UserDto?> GetByPhoneNumber(string phoneNumber);
        ResponseModelDto<int> Create(UserCreateRequestDto request);
        ResponseModelDto<NoContent> Update(int userId, UserUpdateRequestDto request);
        ResponseModelDto<NoContent> Delete(int userId);
    }
}
