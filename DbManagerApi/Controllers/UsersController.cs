using DbManagerApi.Controllers.Filters.FilterAttributes;
using DbManagerApi.Services.Abstractions;
using DbManagerApi.Services.ModuleServices;
using DbManagerApi.Services.UserServices;
using FluentResults;
using Infrastructure;
using Infrastructure.Models;
using Infrastructure.Models.ModelsDTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MR.AspNetCore.Pagination;

namespace DbManagerApi.Controllers;

[ApiController]
[Authorize(Roles = $"{RoleNames.Admin}, {RoleNames.Manager}")]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private UserServiceCursor UserService { get; set; }
    public UsersController(SpellTestDbContext context, IPaginationService paginationService)
    {
        UserService = new UserServiceCursor(context, paginationService);
    }

    [AllowAnonymous]
    [HttpPost]
    [ProducesResponseType(typeof(UserResponseDTO), StatusCodes.Status200OK)]
    public async Task<ActionResult<UserResponseDTO>> AddUser([FromBody] UserCreateDTO dto)
    {
        Result<UserResponseDTO> result = await UserService.CreateEntityAsync(dto);
        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }

        return BadRequest(result.Errors);
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
        var result = await UserService.GetKeysetPaginationAsync(after, propName, limit, userId, reverse);
        if (result.IsSuccess)
        {
            Response.Headers.Append("After", await UserService.GetCursorBase64StringAsync(result.Value.Data.LastOrDefault(), propName));
            return Ok(result.Value);
        }

        return BadRequest(result.Errors);
    }


    [HttpGet("{userId:int}")]
    [ProducesResponseType(typeof(UserResponseDTO), StatusCodes.Status200OK)]
    public async Task<ActionResult<UserResponseDTO>> GetUserById(int userId)
    {
        Result<UserResponseDTO> result = await UserService.GetEntityByIdAsync(userId);
        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }

        return BadRequest(result.Errors);
    }

    [HttpPut("{userId:int}")]
    [UserOwnership("userId", "Users")]
    [ProducesResponseType(typeof(UserResponseDTO), StatusCodes.Status200OK)]
    public async Task<ActionResult<UserResponseDTO>> UpdateUserById(int userId, [FromBody] UserUpdateDTO dto)
    {
        Result<UserResponseDTO> result = await UserService.UpdateEntityAsync(dto, userId);
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
        Result<UserResponseDTO> result = await UserService.DeleteEntityAsync(userId);
        if (result.IsSuccess)
        {
            return Ok();
        }

        return BadRequest(result.Errors);
    }
}
