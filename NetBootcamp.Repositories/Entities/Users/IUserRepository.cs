namespace NetBootcamp.Repositories.Entities.Users
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<User> GetByPhoneNumber(string phoneNumber);
    }
}
