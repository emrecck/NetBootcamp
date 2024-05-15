
namespace NetBootcamp.API.Users
{
    public class UserRepository : IUserRepository
    {
        static readonly List<User> _userList =
        [
            new User{ Id=1, Name="Emre",Surname="Çiçek",PhoneNumber="12345678910",Email="emre@gmail.com"},
            new User{ Id=2, Name="Yunus",Surname="Çiçek",PhoneNumber="67891012345",Email="yunus@gmail.com"},
            new User{ Id=3, Name="Şeyma",Surname="Çiçek",PhoneNumber="56789123410",Email="seyma@gmail.com"},
        ];
        
        public List<User> GetAll()
        {
            return _userList;
        }

        public User? GetById(int id)
        {
            return _userList.Find(x => x.Id == id);
        }

        public void Create(User user)
        {
            _userList.Add(user);
        }

        public void Update(User user)
        {
            var index = _userList.FindIndex(x => x.Id == user.Id);
            _userList[index] = user;
        }

        public void Delete(int userId)
        {
            var deletedItem = GetById(userId);
            _userList.Remove(deletedItem!);
        }
    }
}
