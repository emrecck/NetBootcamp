using NetBootcamp.API.Products;

namespace NetBootcamp.API.Users
{
    public interface IUserRepository
    {
        IReadOnlyList<User> GetAll();
        User? GetById(int id);
        User? GetByPhoneNumber(string phoneNumber);
        void Create(User user);
        void Update(User user);
        void Delete(int userId);
    }
}
