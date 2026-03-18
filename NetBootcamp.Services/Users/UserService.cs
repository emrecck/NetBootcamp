using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NetBootcamp.Repository.Identity;
using NetBootcamp.Repository.Repositories;
using NetBootcamp.Repository.Tokens;
using NetBootcamp.Services.SharedDTOs;
using NetBootcamp.Services.Token.Dtos;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Channels;
using TokenOptions = NetBootcamp.Services.Token.TokenOptions;

namespace NetBootcamp.Services.Users;

public class UserService(IUnitOfWork unitOfWork, UserManager<AppUser> userManager, RoleManager<AppRole> roleManager, IRefreshTokenRepository refreshTokenRepository, IOptions<TokenOptions> tokenOptions, Channel<UserCreatedEvent> userCreatedEventChannel) : IUserService
{
    public async Task<ResponseModelDto<Guid>> SignUp(SignUpRequestDto requestDto)
    {
        var user = new AppUser
        {
            UserName = requestDto.Username,
            Email = requestDto.Email,
            PhoneNumber = requestDto.Phone,
            Name = requestDto.Name,
            Surname = requestDto.Surname,
            BirthDate = requestDto.BirthDate
        };

        var result = await userManager.CreateAsync(user, requestDto.Password);

        if (!result.Succeeded)
        {
            return ResponseModelDto<Guid>.Fail(result.Errors.Select(x => x.Description).ToList());
        }

        if (user.BirthDate.HasValue)
            await userManager.AddClaimAsync(user, new Claim(ClaimTypes.DateOfBirth, user.BirthDate.Value.ToShortDateString()));

        var userCreatedEvent = new UserCreatedEvent(user.Email);
        userCreatedEventChannel.Writer.TryWrite(userCreatedEvent);

        return ResponseModelDto<Guid>.Success(user.Id, System.Net.HttpStatusCode.Created);
    }
    
    public async Task<ResponseModelDto<TokenResponseDto>> SignIn(SignInRequestDto requestDto)
    {
        var user = await userManager.FindByEmailAsync(requestDto.Email);
        if (user == null)
        {
            return ResponseModelDto<TokenResponseDto>.Fail("Invalid email or password.", System.Net.HttpStatusCode.NotFound);
        }

        var isPasswordValid = await userManager.CheckPasswordAsync(user, requestDto.Password);
        if (!isPasswordValid)
        {
            return ResponseModelDto<TokenResponseDto>.Fail("Invalid email or password.", System.Net.HttpStatusCode.BadRequest);
        }

        List<Claim> userClaimList = await CreateUserClaimList(user);

        CreateAccessToken(userClaimList, out DateTime tokenExpiration, out string token);

        RefreshToken? refreshToken = await CreateOrUpdateRefreshToken(user);
        return ResponseModelDto<TokenResponseDto>.Success(new TokenResponseDto(token, refreshToken.Code.ToString(), tokenExpiration));
    }

    public async Task<ResponseModelDto<TokenResponseDto>> SignInByRefreshToken(SignInByRefreshTokenRequestDto requestDto)
    {
        var refreshToken = await refreshTokenRepository.GetAsync(rt => rt.Code.ToString() == requestDto.Code);
        if (refreshToken == null)
        {
            return ResponseModelDto<TokenResponseDto>.Fail("Invalid refresh token.");
        }

        if (refreshToken.Expire < DateTime.Now)
        {
            return ResponseModelDto<TokenResponseDto>.Fail("Refresh token has expired.");
        }

        var user = await userManager.FindByIdAsync(refreshToken.UserId.ToString());
        if (user == null)
        {
            return ResponseModelDto<TokenResponseDto>.Fail("User not found.", System.Net.HttpStatusCode.NotFound);
        }

        List<Claim> userClaimList = await CreateUserClaimList(user);

        CreateAccessToken(userClaimList, out DateTime tokenExpiration, out string token);

        refreshToken = await CreateOrUpdateRefreshToken(user);
        return ResponseModelDto<TokenResponseDto>.Success(new TokenResponseDto(token, refreshToken.Code.ToString(), tokenExpiration));
    }

    private async Task<List<Claim>> CreateUserClaimList(AppUser user)
    {
        var userClaimList = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.Email, user.Email)
        };

        tokenOptions.Value.Audiences.ToList().ForEach(audience =>
        {
            userClaimList.Add(new Claim(JwtRegisteredClaimNames.Aud, audience));
        });


        var userRoles = await userManager.GetRolesAsync(user);
        foreach (var userRoleName in userRoles)
        {
            userClaimList.Add(new Claim(ClaimTypes.Role, userRoleName));

            var roleEntity = await roleManager.FindByNameAsync(userRoleName);
            var roleClaims = await roleManager.GetClaimsAsync(roleEntity!);
            foreach (var roleClaim in roleClaims)
            {
                userClaimList.Add(roleClaim);
            }
        }

        var userClaims = await userManager.GetClaimsAsync(user);
        foreach (var claim in userClaims)
        {
            userClaimList.Add(claim);
        }

        return userClaimList;
    }

    private void CreateAccessToken(List<Claim> userClaimList, out DateTime tokenExpiration, out string token)
    {
        tokenExpiration = DateTime.Now.AddHours(tokenOptions.Value.ExpireByHour);
        var signingCredentials = new SigningCredentials(
            new SymmetricSecurityKey(
                System.Text.Encoding.UTF8.GetBytes(tokenOptions.Value.SignatureKey)
            ),
            SecurityAlgorithms.HmacSha256Signature
        );

        var jwtToken = new JwtSecurityToken(
            claims: userClaimList,
            expires: tokenExpiration,
            signingCredentials: signingCredentials,
            issuer: tokenOptions.Value.Issuer);

        var handler = new JwtSecurityTokenHandler();
        token = handler.WriteToken(jwtToken);
    }

    private async Task<RefreshToken?> CreateOrUpdateRefreshToken(AppUser user)
    {
        var refreshToken = await refreshTokenRepository.GetAsync(x => x.UserId == user.Id);

        if (refreshToken == null)
        {
            refreshToken = new RefreshToken
            {
                UserId = user.Id,
                Code = Guid.NewGuid(),
                Expire = DateTime.Now.AddDays(tokenOptions.Value.RefreshTokenExpireByDay)
            };
            await refreshTokenRepository.CreateAsync(refreshToken);
        }
        else
        {
            refreshToken.Code = Guid.NewGuid();
            refreshToken.Expire = DateTime.Now.AddDays(tokenOptions.Value.RefreshTokenExpireByDay);
            await refreshTokenRepository.UpdateAsync(refreshToken);
        }

        await unitOfWork.CommitAsync();
        return refreshToken;
    }

}