using ChitChatApp.Core.Domain.Entity;

namespace ChitChatApp.Core.Application.Interfaces;

public interface IAuthService
{
    Task<bool> LoginAsync(string email, string password);
    Task<bool> RegisterAsync(string email, string password, string username, string fullName);
    Task LogoutAsync();
    Task<Users?> GetCurrentUserAsync();
    Guid? GetCurrentUserId();
    bool IsAuthenticated { get; }
}