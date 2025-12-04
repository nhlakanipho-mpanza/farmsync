using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FarmSync.Application.DTOs.Auth;
using FarmSync.Application.Interfaces;

namespace FarmSync.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly ILogger<AuthController> _logger;

    public AuthController(IAuthService authService, ILogger<AuthController> logger)
    {
        _authService = authService;
        _logger = logger;
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
    {
        try
        {
            var result = await _authService.LoginAsync(request);
            if (result == null)
                return Unauthorized(new { message = "Invalid username or password" });

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during login");
            return StatusCode(500, new { message = "An error occurred during login" });
        }
    }

    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register([FromBody] RegisterUserDto request)
    {
        try
        {
            var result = await _authService.RegisterAsync(request);
            if (!result)
                return BadRequest(new { message = "User already exists" });

            return Ok(new { message = "User registered successfully" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during registration");
            return StatusCode(500, new { message = "An error occurred during registration" });
        }
    }
}
