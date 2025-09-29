using Application.Interfaces;
using Application.ModelsDTO;
using DomainData.Roles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DbManagerApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = $"{nameof(RoleNames.Admin)}")]
public class RolesController : ControllerBase
{
    IRoleService RoleService { get; init; }

    public RolesController(IRoleService roleService)
    {
        RoleService = roleService;
    }

    [HttpPost]
    [ProducesResponseType(typeof(RoleResponseDTO), StatusCodes.Status200OK)]
    public async Task<ActionResult<RoleResponseDTO>> CreateRole(
        [FromBody] RoleCreateDTO dto)
    {
        RoleResponseDTO result = await RoleService.CreateRoleAsync(dto);
        return Ok(result);
    }

    [HttpGet]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<RoleResponseDTO>>> GetAllRoles()
    {
        IEnumerable<RoleResponseDTO> result = await RoleService.GetRolesAsync();
        return Ok(result);
    }

    [HttpGet("{roleId:int}")]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<RoleResponseDTO>>> GetRole(int roleId)
    {
        RoleResponseDTO result = await RoleService.GetRoleAsync(roleId);
        return Ok(result);
    }


    [HttpPut]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<RoleResponseDTO>>> UpdateRole(
        [FromBody] RoleUpdateDTO dto)
    {
        RoleResponseDTO result = await RoleService.UpdateRoleAsync(dto);
        return Ok(result);
    }

    [HttpDelete("{roleId:int}")]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    public async Task<ActionResult<RoleResponseDTO>> DeleteRole(int roleId)
    {
        RoleResponseDTO result = await RoleService.DeleteRoleAsync(roleId);
        return Ok(result);
    }


}
