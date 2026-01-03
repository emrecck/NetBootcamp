namespace NetBootcamp.Web.Services.Token;

public record CreateClientCredentialTokenRequestDto(string ClientId, string ClientSecret);