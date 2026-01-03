using NetBootcamp.Web.Models;
using System.Net;
using System.Net.Http.Headers;

namespace NetBootcamp.Web.Services.Token;

public class ClientCredentialTokenInterceptor(TokenService tokenService) : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var tokenResponse = await tokenService.GetTokenWithClientCredentialAsync();
        if (!tokenResponse.IsSuccess)
            throw new Exception("Token alınırken bir hata oluştu.");

        request.Headers.Authorization =
            new AuthenticationHeaderValue("Bearer", tokenResponse.Data!.AccessToken);

        var response = await base.SendAsync(request, cancellationToken);

        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            tokenService.RemoveTokenFromCache();

            tokenResponse = await tokenService.GetTokenWithClientCredentialAsync();

            if (!tokenResponse.IsSuccess)
            {
                throw new Exception("Token alınırken bir hata oluştu.");
            }

            request.Headers.Authorization =
            new AuthenticationHeaderValue("Bearer", tokenResponse.Data!.AccessToken);

            response = await base.SendAsync(request, cancellationToken);
        }

        return response;
    }
}
