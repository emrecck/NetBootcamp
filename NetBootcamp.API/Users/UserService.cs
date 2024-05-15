using NetBootcamp.API.DTOs;
using NetBootcamp.API.Users.DTOs;
using System.Collections.Immutable;
using System.Net;

namespace NetBootcamp.API.Users
{
    public class UserService(IUserRepository userRepository) : IUserService
    {
        private readonly IUserRepository _userRepository = userRepository;

        public ResponseModelDto<IReadOnlyList<UserDto>> GetAll()
        {
            var response = _userRepository.GetAll().Select(user => new UserDto(
                user.Id,
                user.Name,
                user.Surname,
                user.PhoneNumber,
                user.Email)).ToImmutableList();
            return ResponseModelDto<IReadOnlyList<UserDto>>.Success(response);
        }

        public ResponseModelDto<UserDto?> GetById(int id)
        {
            var user = _userRepository.GetById(id);
            if (user is null)
                return ResponseModelDto<UserDto?>.Fail("Kullanıcı bulunamadı !", HttpStatusCode.NotFound);

            var newUserDto = new UserDto(
                user.Id,
                user.Name,
                user.Surname,
                user.PhoneNumber,
                user.Email
                );
            return ResponseModelDto<UserDto?>.Success(newUserDto);
        }

        public ResponseModelDto<UserDto?> GetByPhoneNumber(string phoneNumber)
        {
            var user = _userRepository.GetByPhoneNumber(phoneNumber);
            if (user is null)
                return ResponseModelDto<UserDto?>.Fail("Kullanıcı bulunamadı !", HttpStatusCode.NotFound);

            var newUserDto = new UserDto(
                user.Id,
                user.Name,
                user.Surname,
                user.PhoneNumber,
                user.Email
                );
            return ResponseModelDto<UserDto?>.Success(newUserDto);
        }

        public ResponseModelDto<int> Create(UserCreateRequestDto request)
        {
            var isExist = _userRepository.GetByPhoneNumber(request.PhoneNumber);
            if (isExist is not null)
                return ResponseModelDto<int>.Fail("Kullanıcı zaten mevcut");    // default returns bad request status code 

            var newUser = new User
            {
                Id = _userRepository.GetAll().Count + 1,
                Name = request.Name,
                Surname = request.Surname,
                PhoneNumber = request.PhoneNumber,
                Email = request.Email
            };
            _userRepository.Create(newUser);
            return ResponseModelDto<int>.Success(newUser.Id, HttpStatusCode.Created);
        }

        public ResponseModelDto<NoContent> Update(int userId, UserUpdateRequestDto request)
        {
            var isExist = _userRepository.GetById(userId);
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

            _userRepository.Update(updatedUser);
            return ResponseModelDto<NoContent>.Success(HttpStatusCode.NoContent);
        }

        public ResponseModelDto<NoContent> Delete(int userId)
        {
            var isExist = _userRepository.GetById(userId);
            if (isExist is null)
                return ResponseModelDto<NoContent>.Fail("Silmek istediğiniz kullanıcı bulunamadı !", HttpStatusCode.NotFound);

            _userRepository.Delete(userId);
            return ResponseModelDto<NoContent>.Success(HttpStatusCode.NoContent);
        }
    }
}
