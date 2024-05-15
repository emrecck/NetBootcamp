using NetBootcamp.API.Products;

namespace NetBootcamp.API.Users
{
    public interface IUserRepository
    {
        List<User> GetAll();
        User? GetById(int id);
        void Create(User user);
        void Update(User user);
        void Delete(int userId);
    }
}
