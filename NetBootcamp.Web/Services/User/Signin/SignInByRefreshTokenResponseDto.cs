namespace NetBootcamp.Web.Services.User.Signin;

public record SignInByRefreshTokenResponseDto(string AccessToken, string RefreshToken, DateTime ExpireAt);
