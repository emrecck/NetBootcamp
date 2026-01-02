using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetBootcamp.API.Controllers;
using NetBootcamp.Repository.Identity;
using NetBootcamp.Services.Users;

namespace NetBootcamp.API.Users
{
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
}