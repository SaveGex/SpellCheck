using Application.Interfaces;
using Application.ModelsDTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DbManagerApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    public IAuthService AuthService { get; init; }

    public AuthController(IAuthService authService)
    {
        AuthService = authService;
    }

    [HttpGet("ping")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult Hello() => Ok();

    [HttpPost("register")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(UserResponseDTO), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<UserResponseDTO>> Register(
        string? ipAddress,
        [FromBody] UserRegisterDTO userRegisterDTO)
    {
        try
        {
            var result = await AuthService.RegisterUserAsync(userRegisterDTO);
            return StatusCode(StatusCodes.Status201Created, result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("login")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(AuthResponseDTO), StatusCodes.Status200OK)]
    public async Task<ActionResult<AuthResponseDTO>> Login(

        [FromBody] UserLoginDTO userLoginDTO)
    {
        AuthResponseDTO authResponseDTO;
        string? ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();
        try
        {
            authResponseDTO = await AuthService.AuthenticateUserAsync(userLoginDTO, ipAddress ?? throw new Exception("unknown ip address"));
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
        return Ok(authResponseDTO);
    }

    [HttpPost("refresh-token")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(AuthResponseDTO), StatusCodes.Status200OK)]
    public async Task<ActionResult<AuthResponseDTO>> RefreshToken(
        [FromBody] RefreshTokenRequestDTO refreshTokenRequestDTO)
    {
        string? ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();
        AuthResponseDTO result;
        try
        {
            result = await AuthService.RefreshTokenAsync(refreshTokenRequestDTO.RefreshToken, refreshTokenRequestDTO.ClientId, ipAddress ?? throw new Exception("unknown ip address"));
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
        return Ok(result);
    }


}
