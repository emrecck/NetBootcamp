using NetBootcamp.Services.SharedDTOs;
using NetBootcamp.Services.Token;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetBootcamp.Services.Users
{
    public interface IUserService
    {
        Task<ResponseModelDto<Guid>> SignUp(SignUpRequestDto requestDto);
        Task<ResponseModelDto<TokenResponseDto>> SignIn(SignInRequestDto requestDto);
    }
}
