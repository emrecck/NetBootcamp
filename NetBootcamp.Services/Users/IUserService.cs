using NetBootcamp.Services.SharedDTOs;
using NetBootcamp.Services.Token.Dtos;

namespace NetBootcamp.Services.Users;

public interface IUserService
{
    Task<ResponseModelDto<Guid>> SignUp(SignUpRequestDto requestDto);
    Task<ResponseModelDto<TokenResponseDto>> SignIn(SignInRequestDto requestDto);
    Task<ResponseModelDto<TokenResponseDto>> SignInByRefreshToken(SignInByRefreshTokenRequestDto requestDto);
}
