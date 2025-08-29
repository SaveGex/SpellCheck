using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.AspNetCore.Mvc;

namespace DbManagerApi.Authentication.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    [HttpGet]
    [Authorize]
    public Task<IActionResult> Hello()
    {
        return Task.FromResult<IActionResult>(Ok());
    }
}
