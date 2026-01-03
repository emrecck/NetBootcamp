namespace NetBootcamp.Web.Models;

public class SigninViewModel
{
    public string Email { get; set; } = default!;
    public string Password { get; set; } = default!;
    public bool RememberMe { get; set; }
}
