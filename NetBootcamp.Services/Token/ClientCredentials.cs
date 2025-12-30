namespace NetBootcamp.Services.Token;

public class ClientCredentials
{
    public List<Credential> Credentials { get; set; } = default!;
}

public record Credential(string ClientId, string ClientSecret);
