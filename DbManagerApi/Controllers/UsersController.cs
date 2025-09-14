using DbManagerApi.Controllers.Filters.FilterAttributes;
using FluentResults;
using DomainData;
using DomainData.Models;
using DomainData.Models.ModelsDTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MR.AspNetCore.Pagination;
using Application.Interfaces;
using DomainData.Records;

namespace DbManagerApi.Controllers;

[ApiController]
[Authorize(Roles = $"{nameof(RoleNames.Admin)}, {nameof(RoleNames.Manager)}")]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private IUserService UserService { get; set; }
    public UsersController(IUserService userService)
    {
        UserService = userService;
    }

    [AllowAnonymous]
    [HttpPost]
    [ProducesResponseType(typeof(UserResponseDTO), StatusCodes.Status200OK)]
    public async Task<ActionResult<UserResponseDTO>> AddUser([FromBody] UserCreateDTO dto)
    {
        UserResponseDTO result;
        try
        {
            result = await UserService.CreateUserAsync(dto);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }

        return Ok(result);
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
    public async Task<ActionResult<UserResponseDTO>> GetUserById(int userId)
    {
        UserResponseDTO result;
        try
        {
            result = await UserService.GetUserByIdAsync(userId);
        }
        catch(Exception ex)
        {
            return Ok(ex.Message);
        }

        return BadRequest(result);
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
    [ProducesResponseType(typeof(UserResponseDTO), StatusCodes.Status200OK)]
    public async Task<ActionResult<UserResponseDTO>> DeleteUserById(int userId)
    {
        Result<UserResponseDTO> result = await UserService.DeleteUserAsync(userId);
        if (result.IsSuccess)
        {
            return Ok();
        }

        return BadRequest(result.Errors);
    }
}
