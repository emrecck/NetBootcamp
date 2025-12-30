using NetBootcamp.Services.SharedDTOs;

namespace NetBootcamp.Services.Token;

public interface ITokenService
{
    Task<ResponseModelDto<TokenResponseDto>> GenerateTokenAsync(AccessTokenRequestDto requestDto);
}
