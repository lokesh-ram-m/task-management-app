using Microsoft.AspNetCore.Mvc;
using TaskManagementApi.DTOs.Authentication;
using TaskManagementApi.Services.Auth;

namespace TaskManagementApi.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequest request)
    {
        LoginResponse loginResponse = this._authService.Login(request);
        
        return Ok(loginResponse);
    }

    [HttpPost("register")]
    public IActionResult Register([FromBody] RegisterRequest request)
    {
        this._authService.Register(request);

        return Created();
    }
}
