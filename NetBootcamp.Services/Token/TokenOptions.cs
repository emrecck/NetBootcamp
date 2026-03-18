namespace NetBootcamp.Services.Token;

public class TokenOptions
{
    public string SignatureKey { get; set; } = default!;
    public int ExpireByHour { get; set; }
    public int RefreshTokenExpireByDay { get; set; }
    public string Issuer { get; set; } = default!;
    public string[] Audiences { get; set; } = default!;
}
