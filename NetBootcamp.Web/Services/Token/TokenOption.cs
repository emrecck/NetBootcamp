namespace NetBootcamp.Web.Services.Token;

public record class TokenOption(string ClientId, string ClientSecret)
{
    public TokenOption() : this(string.Empty, string.Empty)
    {
    }
}