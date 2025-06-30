using Microsoft.AspNetCore.Mvc;
using ChitChatApp.Core.Application.Interfaces;
using ChitChatApp.Core.Domain.Entity;

namespace ChitChatApp.Core.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    /// <summary>
    /// Register a new user
    /// </summary>
    /// <param name="request">Registration request</param>
    /// <returns>Success or failure</returns>
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        try
        {
            var result = await _authService.RegisterAsync(
                request.Email, 
                request.Password, 
                request.Username, 
                request.FullName);

            if (result)
            {
                return Ok(new { message = "Registration successful", success = true });
            }

            return BadRequest(new { message = "Registration failed", success = false });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = $"Registration failed: {ex.Message}", success = false });
        }
    }

    /// <summary>
    /// Login user
    /// </summary>
    /// <param name="request">Login request</param>
    /// <returns>Success or failure</returns>
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        try
        {
            var result = await _authService.LoginAsync(request.Email, request.Password);

            if (result)
            {
                var currentUser = await _authService.GetCurrentUserAsync();
                return Ok(new 
                { 
                    message = "Login successful", 
                    success = true, 
                    user = currentUser 
                });
            }

            return Unauthorized(new { message = "Invalid credentials", success = false });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = $"Login failed: {ex.Message}", success = false });
        }
    }

    /// <summary>
    /// Logout current user
    /// </summary>
    /// <returns>Success message</returns>
    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        try
        {
            await _authService.LogoutAsync();
            return Ok(new { message = "Logout successful", success = true });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = $"Logout failed: {ex.Message}", success = false });
        }
    }

    /// <summary>
    /// Get current user information
    /// </summary>
    /// <returns>Current user details</returns>
    [HttpGet("me")]
    public async Task<IActionResult> GetCurrentUser()
    {
        try
        {
            var user = await _authService.GetCurrentUserAsync();
            
            if (user != null)
            {
                return Ok(new { user = user, success = true });
            }

            return Unauthorized(new { message = "No authenticated user", success = false });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = $"Failed to get user: {ex.Message}", success = false });
        }
    }

    /// <summary>
    /// Check if user is authenticated
    /// </summary>
    /// <returns>Authentication status</returns>
    [HttpGet("status")]
    public IActionResult GetAuthStatus()
    {
        return Ok(new 
        { 
            isAuthenticated = _authService.IsAuthenticated, 
            userId = _authService.GetCurrentUserId(),
            success = true 
        });
    }
}

public class RegisterRequest
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
}

public class LoginRequest
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
