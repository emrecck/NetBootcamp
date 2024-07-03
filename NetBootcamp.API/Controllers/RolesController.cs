using Microsoft.AspNetCore.Mvc;
using NetBootcamp.Service.Roles;
using NetBootcamp.Service.Roles.DTOs;
using NetBootcamp.Service.Roles.RoleCreateUseCase;

namespace NetBootcamp.API.Controllers
{
    public class RolesController(IRoleService roleService) : CustomBaseController
    {
        private readonly IRoleService _roleService = roleService;

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return CreateActionResult(await _roleService.GetAll());
        }

        [HttpGet("{roleId}")]
        public async Task<IActionResult> Get(int roleId)
        {
            return CreateActionResult(await _roleService.GetById(roleId));
        }

        [HttpPost]
        public async Task<IActionResult> Create(RoleCreateRequestDto request)
        {
            var result = await _roleService.Create(request);

            return CreateActionResult(result, nameof(Get), new { roleId = result.Data });
        }

        [HttpPut("{roleId}")]
        public async Task<IActionResult> Update(int roleId, RoleUpdateRequestDto request)
        {
            return CreateActionResult(await _roleService.Update(roleId, request));
        }

        [HttpDelete("{roleId}")]
        public async Task<IActionResult> Delete(int roleId)
        {
            return CreateActionResult(await _roleService.Delete(roleId));
        }
    }
}
