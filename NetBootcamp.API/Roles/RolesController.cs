using Microsoft.AspNetCore.Mvc;
using NetBootcamp.API.Controllers;
using NetBootcamp.API.Roles.DTOs;
using NetBootcamp.API.Users;

namespace NetBootcamp.API.Roles
{
    public class RolesController(IRoleService roleService) : CustomBaseController
    {
        private readonly IRoleService _roleService = roleService;

        [HttpGet]
        public IActionResult GetAll()
        {
            return CreateActionResult(_roleService.GetAll());
        }

        [HttpGet("{roleId}")]
        public IActionResult Get(int roleId)
        {
            return CreateActionResult(_roleService.GetById(roleId));
        }

        [HttpPost]
        public IActionResult Create(RoleCreateRequestDto request)
        {
            var result = _roleService.Create(request);

            return CreateActionResult(result, nameof(Get), new { roleId = result.Data });
        }

        [HttpPut("{roleId}")]
        public IActionResult Update(int roleId, RoleUpdateRequestDto request)
        {
            return CreateActionResult(_roleService.Update(roleId, request));
        }

        [HttpDelete("{roleId}")]
        public IActionResult Delete(int roleId)
        {
            return CreateActionResult(_roleService.Delete(roleId));
        }
    }
}
