using Microsoft.AspNetCore.Mvc;
using NetBootcamp.Service.Users;
using NetBootcamp.Service.Users.DTOs;
using NetBootcamp.Service.Users.UserCreateUseCase;

namespace NetBootcamp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController(IUserService userService) : CustomBaseController
    {
        private readonly IUserService _userService = userService;

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return CreateActionResult(await _userService.GetAll());
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> Get(int userId)
        {
            return CreateActionResult(await _userService.GetById(userId));
        }

        [HttpPost]
        public async Task<IActionResult> Create(UserCreateRequestDto request)
        {
            var result = await _userService.Create(request);

            return CreateActionResult(result, nameof(Get), new { userId = result.Data });
        }

        [HttpPut("{userId:int}")]
        public async Task<IActionResult> Update(int userId, UserUpdateRequestDto request)
        {
            return CreateActionResult(await _userService.Update(userId, request));
        }

        [HttpDelete("{userId}")]
        public async Task<IActionResult> Delete(int userId)
        {
            return CreateActionResult(await _userService.Delete(userId));
        }
    }
}
