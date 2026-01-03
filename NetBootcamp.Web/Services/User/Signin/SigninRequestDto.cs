namespace NetBootcamp.Web.Services.User.Signin;

public record SigninRequestDto(string Email, string Password, bool RememberMe);
