using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using NetBootcamp.Web.Models;

namespace NetBootcamp.Web.Services.Token;

public class TokenService(HttpClient client, IOptions<TokenOption> options, IMemoryCache memoryCache)
{
    private const string TokenKey = "client_credential:access_token";

    public async Task<ServiceResponseModel<CreateClientCredentialTokenResponseDto>> GetTokenWithClientCredentialAsync()
    {
        if (memoryCache.TryGetValue(TokenKey, out var cachedTokenObj) && cachedTokenObj is string accessToken)
        {
            return ServiceResponseModel<CreateClientCredentialTokenResponseDto>.Success(new CreateClientCredentialTokenResponseDto(accessToken));
        }

        var requestDto = new CreateClientCredentialTokenRequestDto(
            options.Value.ClientId,
            options.Value.ClientSecret
        );

        var response = await client.PostAsJsonAsync("/api/token/CreateClientCredential", requestDto);

        var responseAsBody = await response.Content.ReadFromJsonAsync<ResponseModelDto<CreateClientCredentialTokenResponseDto>>();

        if (!response.IsSuccessStatusCode)
        {
            return ServiceResponseModel<CreateClientCredentialTokenResponseDto>.Fail(responseAsBody.FailMessages);
        }

        memoryCache.Set(TokenKey, responseAsBody!.Data!.AccessToken, TimeSpan.FromHours(9));

        return ServiceResponseModel<CreateClientCredentialTokenResponseDto>.Success(responseAsBody.Data);
    }

    public void RemoveTokenFromCache()
    {
        memoryCache.Remove(TokenKey);
    }
}