using Microsoft.AspNetCore.Mvc;
using NetBootcamp.API.Controllers;
using NetBootcamp.API.Users;

namespace NetBootcamp.API.Roles
{
    public class RolesController : CustomBaseController
    {
        private readonly IRoleService _roleService;

        public RolesController(IRoleService roleService)
        {
            _roleService = roleService;
        }

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
        public IActionResult Create(Role request)
        {
            var result = _roleService.Create(request);

            return CreateActionResult(result, nameof(Get), new { roleId = result.Data });
        }

        [HttpPut("{roleId}")]
        public IActionResult Update(int roleId, Role request)
        {
            return CreateActionResult(_roleService.Update(roleId, request));
        }

        [HttpDelete]
        public IActionResult Delete(int roleId)
        {
            return CreateActionResult(_roleService.Delete(roleId));
        }
    }
}
