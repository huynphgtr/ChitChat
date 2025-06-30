using ChitChatApp.Core.Domain.Entity;

namespace ChitChatApp.Core.Application.Interfaces;

public interface IUserRepository
{
    Task<Users?> GetUserByIdAsync(Guid id);
    Task<Users?> GetUserByEmailAsync(string email);
    Task<Users?> GetUserByUsernameAsync(string username);
    Task<IEnumerable<Users>> GetAllUsersAsync();
    Task<Users> CreateUserAsync(Users user);
    Task<Users> UpdateUserAsync(Users user);
    Task<bool> DeleteUserAsync(Guid id);
    Task<bool> UserExistsAsync(string email, string username);
}