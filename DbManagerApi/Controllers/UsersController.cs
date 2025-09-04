using DbManagerApi.Controllers.Filters.FilterAttributes;
using DbManagerApi.Services;
using DbManagerApi.Services.Abstractions;
using FluentResults;
using Infrastructure;
using Infrastructure.Models;
using Infrastructure.Models.ModelsDTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DbManagerApi.Controllers;

[ApiController]
[Authorize(Roles = $"{RoleNames.Admin}, {RoleNames.Manager}")]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private UserServiceAbstract UserService { get; set; }
    public UsersController(SpellTestDbContext dbContext)
    {
        UserService = new UserService(dbContext);
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

    [HttpGet("users")]
    [ProducesResponseType(typeof(IEnumerable<UserResponseDTO>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<UserResponseDTO>>> GetAllUsers(
        [FromQuery] string? propName,
        [FromQuery] int? limit,
        [FromQuery] int? userId,
        [FromQuery] bool? reverse)
    {
        var result = await UserService.GetEntitiesSequenceAsync(propName, limit, userId, reverse);
        if (result.IsSuccess)
            return Ok(result.Value);

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
