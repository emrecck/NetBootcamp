using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NetBootcamp.Repository.Repositories;
using NetBootcamp.Repository.Tokens;
using NetBootcamp.Services.SharedDTOs;
using NetBootcamp.Services.Token.Dtos;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace NetBootcamp.Services.Token;

public class TokenService(IUnitOfWork unitOfWork, IRefreshTokenRepository refreshTokenRepository, IOptions<TokenOptions> tokenOptions, IOptions<ClientCredentials> clientCredentials) : ITokenService
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

        tokenOptions.Value.Audiences.ToList().ForEach(audience =>
        {
            claims.Add(new Claim(JwtRegisteredClaimNames.Aud, audience));
        });

        var tokenExpiration = DateTime.Now.AddHours(tokenOptions.Value.ExpireByHour);

        var signingCredentials = new SigningCredentials(
            new SymmetricSecurityKey(
                System.Text.Encoding.UTF8.GetBytes(tokenOptions.Value.SignatureKey)
            ),
            SecurityAlgorithms.HmacSha256Signature
        );

        var jwtToken = new JwtSecurityToken(
            claims: claims,
            expires: tokenExpiration,
            signingCredentials: signingCredentials,
            issuer: tokenOptions.Value.Issuer);

        var handler = new JwtSecurityTokenHandler();
        var token = handler.WriteToken(jwtToken);
        return ResponseModelDto<TokenResponseDto>.Success(new TokenResponseDto(token, "", tokenExpiration));
    }

    public async Task<ResponseModelDto<NoContent>> RevokeRefreshToken(Guid code)
    {
        var hasRefreshToken = await refreshTokenRepository.GetAsync(rt => rt.Code == code);
        if (hasRefreshToken is null)
        {
            return ResponseModelDto<NoContent>.Fail("Refresh token not found.");
        }

        await refreshTokenRepository.DeleteAsync(hasRefreshToken.Id);
        await unitOfWork.CommitAsync();

        return ResponseModelDto<NoContent>.Success();
    } 
}
