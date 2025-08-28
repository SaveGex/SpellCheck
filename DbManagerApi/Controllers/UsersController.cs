using DbManagerApi.Controllers.Filters.FilterAttributes;
using DbManagerApi.Services;
using DbManagerApi.Services.Interfaces;
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
    private IUserService UserService { get; set; }
    public UsersController(SpellTestDbContext dbContext)
    {
        UserService = new UserService(dbContext);
    }

    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> AddUser([FromBody]UserCreateDTO dto)
    {
        Result<UserResponseDTO> result = await  UserService.CreateUserAsync(dto);
        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }
        
        return BadRequest(result.Errors);
    }

    [HttpGet("users")]
    public async Task<IActionResult> GetAllUsers(
        [FromQuery] string? propName,
        [FromQuery] int? limit,
        [FromQuery] int? userId,
        [FromQuery] bool? reverse)
    {
        var result = await UserService.GetUserSequenceAsync(propName, limit, userId, reverse);
        if (result.IsSuccess)
            return Ok(result.Value);

        return BadRequest(result.Errors);
    }


    [HttpGet("{userId:int}")]
    public async Task<IActionResult> GetUserById(int userId)
    {
        Result<UserResponseDTO> result = await UserService.GetUserByIdAsync(userId);
        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }

        return BadRequest(result.Errors);
    }

    [HttpPut("{userId:int}")]
    [UserOwnership("userId", "Users")]
    public async Task<IActionResult> UpdateUserById(int userId, [FromBody]UserUpdateDTO dto)
    {
        Result<UserResponseDTO> result = await UserService.UpdateUserAsync(dto, userId);
        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }
        return BadRequest(result.Errors);
    }

    [HttpDelete("{userId:int}")]
    [UserOwnership("userId", "Users")]
    public async Task<IActionResult> DeleteUserById(int userId)
    {
        Result<UserResponseDTO> result = await UserService.DeleteUserAsync(userId);
        if (result.IsSuccess)
        {
            return Ok();
        }

        return BadRequest(result.Errors);
    }
}
