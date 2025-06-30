using ChitChatApp.Core.Domain.Entity;

namespace ChitChatApp.Core.Application.Interfaces;

public interface IContactRepository
{
    Task<IEnumerable<Users>> GetContactsForUserAsync(Guid userId);
    Task<bool> AddContactAsync(Guid userId, Guid friendId);
    Task<bool> RemoveContactAsync(Guid userId, Guid friendId);
    Task<bool> AreContactsAsync(Guid userId, Guid friendId);
    Task<IEnumerable<Users>> SearchUsersAsync(string searchTerm);
    Task<bool> BlockUserAsync(Guid userId, Guid blockedUserId);
    Task<bool> UnblockUserAsync(Guid userId, Guid blockedUserId);
    Task<IEnumerable<Users>> GetBlockedUsersAsync(Guid userId);
    Task<bool> IsUserBlockedAsync(Guid userId, Guid blockedUserId);
}