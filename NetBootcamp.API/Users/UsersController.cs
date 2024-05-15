using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NetBootcamp.API.Controllers;
using NetBootcamp.API.Users.DTOs;

namespace NetBootcamp.API.Users
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController(IUserService userService) : CustomBaseController
    {
        private readonly IUserService _userService = userService;

        [HttpGet]
        public IActionResult GetAll()
        {
            return CreateActionResult(_userService.GetAll());
        }

        [HttpGet("{userId}")]
        public IActionResult Get(int userId) 
        {
            return CreateActionResult(_userService.GetById(userId));
        }

        [HttpPost]
        public IActionResult Create(UserCreateRequestDto request)
        {
            var result = _userService.Create(request);

            return CreateActionResult(result, nameof(Get), new { userId = result.Data });
        }

        [HttpPut("{userId}")]
        public IActionResult Update(int userId, UserUpdateRequestDto request)
        {
            return CreateActionResult(_userService.Update(userId, request));
        }

        [HttpDelete("{userId}")]
        public IActionResult Delete(int userId) 
        {
            return CreateActionResult(_userService.Delete(userId));
        }
    }
}
