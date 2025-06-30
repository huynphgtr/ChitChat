using Microsoft.AspNetCore.Mvc;
using ChitChatApp.Core.Application.Interfaces;
using ChitChatApp.Core.Domain.Entity;

namespace ChitChatApp.Core.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserRepository _userRepository;

    public UsersController(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    /// <summary>
    /// Get all users
    /// </summary>
    /// <returns>List of all users</returns>
    [HttpGet]
    public async Task<IActionResult> GetAllUsers()
    {
        try
        {
            var users = await _userRepository.GetAllUsersAsync();
            return Ok(new { users = users, success = true });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = $"Failed to get users: {ex.Message}", success = false });
        }
    }

    /// <summary>
    /// Get user by ID
    /// </summary>
    /// <param name="id">User ID</param>
    /// <returns>User details</returns>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserById(Guid id)
    {
        try
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            
            if (user != null)
            {
                return Ok(new { user = user, success = true });
            }

            return NotFound(new { message = "User not found", success = false });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = $"Failed to get user: {ex.Message}", success = false });
        }
    }

    /// <summary>
    /// Get user by email
    /// </summary>
    /// <param name="email">User email</param>
    /// <returns>User details</returns>
    [HttpGet("email/{email}")]
    public async Task<IActionResult> GetUserByEmail(string email)
    {
        try
        {
            var user = await _userRepository.GetUserByEmailAsync(email);
            
            if (user != null)
            {
                return Ok(new { user = user, success = true });
            }

            return NotFound(new { message = "User not found", success = false });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = $"Failed to get user: {ex.Message}", success = false });
        }
    }

    /// <summary>
    /// Get user by username
    /// </summary>
    /// <param name="username">Username</param>
    /// <returns>User details</returns>
    [HttpGet("username/{username}")]
    public async Task<IActionResult> GetUserByUsername(string username)
    {
        try
        {
            var user = await _userRepository.GetUserByUsernameAsync(username);
            
            if (user != null)
            {
                return Ok(new { user = user, success = true });
            }

            return NotFound(new { message = "User not found", success = false });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = $"Failed to get user: {ex.Message}", success = false });
        }
    }

    /// <summary>
    /// Update user
    /// </summary>
    /// <param name="id">User ID</param>
    /// <param name="request">Update request</param>
    /// <returns>Updated user</returns>
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUser(Guid id, [FromBody] UpdateUserRequest request)
    {
        try
        {
            var existingUser = await _userRepository.GetUserByIdAsync(id);
            
            if (existingUser == null)
            {
                return NotFound(new { message = "User not found", success = false });
            }

            // Update fields
            if (!string.IsNullOrEmpty(request.FullName))
                existingUser.full_name = request.FullName;
            
            if (!string.IsNullOrEmpty(request.Username))
                existingUser.user_name = request.Username;
            
            if (!string.IsNullOrEmpty(request.AvatarUrl))
                existingUser.avatar_url = request.AvatarUrl;
            
            if (request.Status.HasValue)
                existingUser.status = request.Status.Value;

            var updatedUser = await _userRepository.UpdateUserAsync(existingUser);
            
            return Ok(new { user = updatedUser, message = "User updated successfully", success = true });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = $"Failed to update user: {ex.Message}", success = false });
        }
    }

    /// <summary>
    /// Delete user
    /// </summary>
    /// <param name="id">User ID</param>
    /// <returns>Success message</returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(Guid id)
    {
        try
        {
            var result = await _userRepository.DeleteUserAsync(id);
            
            if (result)
            {
                return Ok(new { message = "User deleted successfully", success = true });
            }

            return NotFound(new { message = "User not found", success = false });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = $"Failed to delete user: {ex.Message}", success = false });
        }
    }

    /// <summary>
    /// Check if user exists
    /// </summary>
    /// <param name="email">Email</param>
    /// <param name="username">Username</param>
    /// <returns>Existence status</returns>
    [HttpGet("exists")]
    public async Task<IActionResult> CheckUserExists([FromQuery] string email, [FromQuery] string username)
    {
        try
        {
            var exists = await _userRepository.UserExistsAsync(email, username);
            return Ok(new { exists = exists, success = true });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = $"Failed to check user existence: {ex.Message}", success = false });
        }
    }
}

public class UpdateUserRequest
{
    public string? FullName { get; set; }
    public string? Username { get; set; }
    public string? AvatarUrl { get; set; }
    public bool? Status { get; set; }
}
