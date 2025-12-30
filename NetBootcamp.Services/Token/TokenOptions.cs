namespace NetBootcamp.Services.Token;

public class TokenOptions
{
    public string SignatureKey { get; set; } = default!;
    public int ExpireByHour { get; set; }
}
