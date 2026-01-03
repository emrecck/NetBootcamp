using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetBootcamp.API.Controllers;
using NetBootcamp.Services.Users;

namespace NetBootcamp.API.Users;

[Authorize]
public class UsersController(IUserService userService) : CustomBaseController
{
    [HttpPost("signup")]
    public async Task<IActionResult> SignUp(SignUpRequestDto requestDto)
    {
        var result = await userService.SignUp(requestDto);
        return CreateActionResult(result);
    }

    [HttpPost("signin")]
    public async Task<IActionResult> SignIn(SignInRequestDto requestDto)
    {
        var result = await userService.SignIn(requestDto);
        return CreateActionResult(result);
    }
}