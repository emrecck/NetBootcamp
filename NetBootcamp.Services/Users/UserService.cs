using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NetBootcamp.Repository.Identity;
using NetBootcamp.Services.SharedDTOs;
using NetBootcamp.Services.Token;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using TokenOptions = NetBootcamp.Services.Token.TokenOptions;

namespace NetBootcamp.Services.Users;

public class UserService(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager, IOptions<TokenOptions> tokenOptions) : IUserService
{
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

        var userClaimList = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.Email, user.Email)
        };

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

        

        tokenOptions.Value.Audiences.ToList().ForEach(audience =>
        {
            userClaimList.Add(new Claim(JwtRegisteredClaimNames.Aud, audience));
        });

        var tokenExpiration = DateTime.Now.AddHours(tokenOptions.Value.ExpireByHour);

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
        var token = handler.WriteToken(jwtToken);
        return ResponseModelDto<TokenResponseDto>.Success(new TokenResponseDto { AccessToken = token, ExpireAt = tokenExpiration });
    }

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


        return ResponseModelDto<Guid>.Success(user.Id, System.Net.HttpStatusCode.Created);
    }
}