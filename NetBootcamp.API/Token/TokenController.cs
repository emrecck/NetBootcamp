using Microsoft.AspNetCore.Mvc;
using NetBootcamp.API.Controllers;
using NetBootcamp.Services.Token;

namespace NetBootcamp.API.Token;

public class TokenController(ITokenService tokenService) : CustomBaseController
{
    [HttpPost]
    public async Task<IActionResult> GenerateToken(AccessTokenRequestDto requestDto)
    {
        var result = await tokenService.GenerateTokenAsync(requestDto);
        return CreateActionResult(result);
    }
}
