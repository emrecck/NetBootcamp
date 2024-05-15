using NetBootcamp.API.DTOs;
using System.Net;

namespace NetBootcamp.API.Users
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public ResponseModelDto<List<User>> GetAll()
        {
            return ResponseModelDto<List<User>>.Success(_userRepository.GetAll());
        }

        public ResponseModelDto<User?> GetById(int id)
        {
            var user = _userRepository.GetById(id);
            if (user is null)
                return ResponseModelDto<User?>.Fail("Kullanıcı bulunamadı !", HttpStatusCode.NotFound);

            return ResponseModelDto<User?>.Success(user);
        }

        public ResponseModelDto<int> Create(User request)
        {
            var isExist = _userRepository.GetById(request.Id);
            if (isExist is not null)
                return ResponseModelDto<int>.Fail("Kullanıcı zaten mevcut");    // default returns bad request status code 

            _userRepository.Create(request);
            return ResponseModelDto<int>.Success(request.Id);
        }

        public ResponseModelDto<NoContent> Update(int userId, User request)
        {
            var isExist = _userRepository.GetById(userId);
            if (isExist is null)
                return ResponseModelDto<NoContent>.Fail("Güncellemek istediğiniz kullanıcı bulunamadı !", HttpStatusCode.NotFound);

            _userRepository.Update(request);
            return ResponseModelDto<NoContent>.Success();
        }

        public ResponseModelDto<NoContent> Delete(int userId)
        {
            var isExist = _userRepository.GetById(userId);
            if (isExist is null)
                return ResponseModelDto<NoContent>.Fail("Silmek istediğiniz kullanıcı mevcut değil !", HttpStatusCode.NotFound);

            _userRepository.Delete(userId);
            return ResponseModelDto<NoContent>.Success();
        }
    }
}
