using NetBootcamp.Services.SharedDTOs;
using NetBootcamp.Services.Token.Dtos;

namespace NetBootcamp.Services.Token;

public interface ITokenService
{
    Task<ResponseModelDto<TokenResponseDto>> GenerateTokenAsync(AccessTokenRequestDto requestDto);
    Task<ResponseModelDto<NoContent>> RevokeRefreshToken(Guid code);
}
