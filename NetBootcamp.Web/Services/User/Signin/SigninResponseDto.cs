namespace NetBootcamp.Web.Services.User.Signin;

public record SigninResponseDto(string AccessToken, DateTime ExpireAt);