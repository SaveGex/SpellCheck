using Application.Interfaces;
using Application.ModelsDTO;
using DbManagerApi.Controllers.Filters.FilterAttributes;
using DomainData.Records;
using DomainData.Roles;
using FluentResults;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MR.AspNetCore.Pagination;

namespace DbManagerApi.Controllers;

[ApiController]
[Authorize(Roles = $"{nameof(RoleNames.Admin)}")]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private IUserService UserService { get; set; }
    public UsersController(IUserService userService)
    {
        UserService = userService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(KeysetPaginationResult<UserResponseDTO>), StatusCodes.Status200OK)]
    public async Task<ActionResult<KeysetPaginationResult<UserResponseDTO>>> GetAllUsers(
        [FromQuery] string? after,
        [FromQuery] string? propName,
        [FromQuery] int? limit,
        [FromQuery] int? userId,
        [FromQuery] bool? reverse)
    {
        KeysetPaginationAfterResult<UserResponseDTO> result;
        try
        {
            result = await UserService.GetUsersKeysetPaginationAsync(after, propName, limit, userId, reverse);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }

        Response.Headers.Append("After", result.After);
        return Ok(result);
    }


    [HttpGet("{userId:int}")]
    [ProducesResponseType(typeof(UserResponseDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<UserResponseDTO>> GetUserById(int userId)
    {
        try
        {
            var result = await UserService.GetUserByIdAsync(userId);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{userId:int}")]
    [UserOwnership("userId", "Users")]
    [ProducesResponseType(typeof(UserResponseDTO), StatusCodes.Status200OK)]
    public async Task<ActionResult<UserResponseDTO>> UpdateUserById(int userId, [FromBody] UserUpdateDTO dto)
    {
        Result<UserResponseDTO> result = await UserService.UpdateUserAsync(dto);
        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }
        return BadRequest(result.Errors);
    }

    [HttpDelete("{userId:int}")]
    [UserOwnership("userId", "Users")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteUserById(int userId)
    {

        try
        {
            _ = await UserService.DeleteUserAsync(userId);
            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("{userId:int}/roles/{roleId:int}")]
    [Authorize(Roles = $"{nameof(RoleNames.Admin)}")]
    [ProducesResponseType(typeof(UserResponseDTO), StatusCodes.Status200OK)]
    public async Task<ActionResult<UserResponseDTO>> AttachRoleToUser(int userId, int roleId)
    {
        UserResponseDTO result = await UserService.AddRoleToUserAsync(userId, roleId);
        return Ok(result);
    }

    [HttpDelete("{userId:int}/roles/{roleId:int}")]
    [Authorize(Roles = $"{nameof(RoleNames.Admin)}")]
    [ProducesResponseType(typeof(UserResponseDTO), StatusCodes.Status200OK)]
    public async Task<ActionResult<UserResponseDTO>> DettachRoleToUser(int userId, int roleId)
    {
        UserResponseDTO result = await UserService.RemoveRoleFromUserAsync(userId, roleId);
        return Ok(result);
    }
}
