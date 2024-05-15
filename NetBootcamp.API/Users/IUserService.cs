using NetBootcamp.API.DTOs;
using NetBootcamp.API.Products.DTOs;
using NetBootcamp.API.Products;
using System.Collections.Immutable;

namespace NetBootcamp.API.Users
{
    public interface IUserService
    {
        ResponseModelDto<List<User>> GetAll();
        ResponseModelDto<User?> GetById(int id);
        ResponseModelDto<int> Create(User request);
        ResponseModelDto<NoContent> Update(int userId, User request);
        ResponseModelDto<NoContent> Delete(int userId);
    }
}
