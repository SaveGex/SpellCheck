using DbManagerApi.Services;
using DbManagerApi.Services.Interfaces;
using FluentResults;
using Infrastructure;
using Infrastructure.Models;
using Infrastructure.Models.ModelsDTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;

namespace DbManagerApi.Controllers;

[Route("api/[controller]")]
[Authorize(Roles = RoleNames.User)]
public class UsersController : Controller
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

    [HttpGet]
    public async Task<IActionResult> GetAllUsers()
    {
        Result<IEnumerable<UserResponseDTO>> result = await UserService.GetAllUsersAsync();
        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }

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
    public async Task<IActionResult> DeleteUserById(int userId)
    {
        Result result = await UserService.DeleteUserAsync(userId);
        if (result.IsSuccess)
        {
            return Ok();
        }

        return BadRequest(result.Errors);
    }

    //[Authorize(Roles = RoleNames.User)]
    [AllowAnonymous]
    [HttpGet("role/{userId:int}")]
    public async Task<IActionResult> GetUserRoleById(int userId, SpellTestDbContext db)
    {
        var user = await db.Users.FindAsync(userId);
        return Ok(user?.Roles);
    }
}
