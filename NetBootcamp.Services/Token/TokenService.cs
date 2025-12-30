using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NetBootcamp.Services.SharedDTOs;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace NetBootcamp.Services.Token;

public class TokenService(IOptions<TokenOptions> tokenOptions, IOptions<ClientCredentials> clientCredentials) : ITokenService
{
    public async Task<ResponseModelDto<TokenResponseDto>> GenerateTokenAsync(AccessTokenRequestDto requestDto)
    {
        if (!clientCredentials.Value.Credentials.Any(x => x.ClientId == requestDto.ClientId))
        {
            return ResponseModelDto<TokenResponseDto>.Fail("Invalid Client Credentials");
        }

        var claims = new List<Claim>()
        {
            new Claim("client_id", requestDto.ClientId)
        };

        var tokenExpiration = DateTime.Now.AddHours(tokenOptions.Value.ExpireByHour);

        var signingCredentials = new SigningCredentials(
            new SymmetricSecurityKey(
                System.Text.Encoding.UTF8.GetBytes(tokenOptions.Value.SignatureKey)
            ),
            SecurityAlgorithms.HmacSha256Signature
        );

        var jwtToken = new JwtSecurityToken(claims: claims, expires: tokenExpiration, signingCredentials: signingCredentials);

        var handler = new JwtSecurityTokenHandler();
        var token = handler.WriteToken(jwtToken);
        return ResponseModelDto<TokenResponseDto>.Success(new TokenResponseDto { AccessToken = token, ExpireAt = tokenExpiration });
    }


}
