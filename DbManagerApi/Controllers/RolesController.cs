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
    [ProducesResponseType(typeof(RoleResponseDTO), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<RoleResponseDTO>> CreateRole([FromBody] RoleCreateDTO dto)
    {
        try
        {
            var result = await RoleService.CreateRoleAsync(dto);
            return CreatedAtAction(nameof(GetRole), new { roleId = result.Id }, result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<RoleResponseDTO>>> GetAllRoles()
    {
        try
        {
            IEnumerable<RoleResponseDTO> result = await RoleService.GetRolesAsync();
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("{roleId:int}")]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<RoleResponseDTO>>> GetRole(int roleId)
    {
        try
        {
            RoleResponseDTO result = await RoleService.GetRoleAsync(roleId);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }


    [HttpPut]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<RoleResponseDTO>>> UpdateRole(
        [FromBody] RoleUpdateDTO dto)
    {
        try
        {
            RoleResponseDTO result = await RoleService.UpdateRoleAsync(dto);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{roleId:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteRole(int roleId)
    {
        try
        {
            await RoleService.DeleteRoleAsync(roleId);
            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }


}
