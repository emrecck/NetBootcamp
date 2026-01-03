using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using NetBootcamp.Web.Models;
using NetBootcamp.Web.Services.User.Signin;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace NetBootcamp.Web.Services.User;

public class UserService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor, IDataProtectionProvider dataProtectionProvider)
{
    public async Task<ServiceResponseModel<NoContent>> SignIn(SigninRequestDto requestDto)
    {
        var response = await httpClient.PostAsJsonAsync("/api/users/signin", requestDto);
        if (!response.IsSuccessStatusCode)
        {
            return ServiceResponseModel<NoContent>.Fail("Sign in failed");
        }

        var responseAsModel = await response.Content.ReadFromJsonAsync<ResponseModelDto<SigninResponseDto>>();

        var authenticationTokenList = new List<AuthenticationToken>()
        {
            new(){ Name = OpenIdConnectParameterNames.AccessToken, Value = responseAsModel.Data.AccessToken },
            new(){ Name = OpenIdConnectParameterNames.ExpiresIn, Value = responseAsModel.Data.ExpireAt.ToString() },
            new(){ Name = OpenIdConnectParameterNames.RefreshToken, Value = "responseAsModel.Data.RefreshToken" },
        };

        var jwtHandler = new JwtSecurityTokenHandler();
        var jwtSecurityToken = jwtHandler.ReadJwtToken(responseAsModel.Data.AccessToken);

        ClaimsIdentity claimsIdentity = new ClaimsIdentity(jwtSecurityToken.Claims, CookieAuthenticationDefaults.AuthenticationScheme, ClaimTypes.Name, ClaimTypes.Role);

        ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

        var authenticationProperties = new AuthenticationProperties();

        authenticationProperties.StoreTokens(authenticationTokenList);

        await httpContextAccessor.HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            claimsPrincipal,
            authenticationProperties);

        var dataprotector = dataProtectionProvider.CreateProtector("thisIsMyProctectorKey");

        var encryptedbgColor = dataprotector.Protect("red");

        httpContextAccessor.HttpContext.Response.Cookies.Append("bg-color", encryptedbgColor);

        return ServiceResponseModel<NoContent>.Success();
    }

}
