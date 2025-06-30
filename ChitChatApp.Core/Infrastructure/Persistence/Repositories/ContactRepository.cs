using ChitChatApp.Core.Application.Interfaces;
using ChitChatApp.Core.Domain.Entity;
using ChitChatApp.Core.Infrastructure.Supabase;
using SupabaseClient = Supabase.Client;

namespace ChitChatApp.Core.Infrastructure.Persistence.Repositories;

public class ContactRepository : IContactRepository
{
    private readonly SupabaseClient _supabaseClient;

    public ContactRepository(SupabaseService supabaseService)
    {
        _supabaseClient = supabaseService.Client;
    }

    public async Task<IEnumerable<Users>> GetContactsForUserAsync(Guid userId)
    {
        var contacts = await _supabaseClient
            .From<User_contacts>()
            .Where(uc => uc.user_id == userId)
            .Get();

        var friendIds = contacts.Models.Select(c => c.friend_id).ToList();
        
        if (!friendIds.Any())
            return Enumerable.Empty<Users>();

        var users = await _supabaseClient
            .From<Users>()
            .Where(u => friendIds.Contains(u.id))
            .Get();

        return users.Models;
    }

    public async Task<bool> AddContactAsync(Guid userId, Guid friendId)
    {
        try
        {
            // Add both directions of the relationship
            var contact1 = new User_contacts
            {
                user_id = userId,
                friend_id = friendId,
                created_at = DateTime.UtcNow
            };

            var contact2 = new User_contacts
            {
                user_id = friendId,
                friend_id = userId,
                created_at = DateTime.UtcNow
            };

            await _supabaseClient
                .From<User_contacts>()
                .Insert(new[] { contact1, contact2 });

            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> RemoveContactAsync(Guid userId, Guid friendId)
    {
        try
        {
            // Remove both directions of the relationship
            await _supabaseClient
                .From<User_contacts>()
                .Where(uc => (uc.user_id == userId && uc.friend_id == friendId) ||
                           (uc.user_id == friendId && uc.friend_id == userId))
                .Delete();

            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> AreContactsAsync(Guid userId, Guid friendId)
    {
        var result = await _supabaseClient
            .From<User_contacts>()
            .Where(uc => uc.user_id == userId && uc.friend_id == friendId)
            .Get();

        return result.Models.Any();
    }

    public async Task<IEnumerable<Users>> SearchUsersAsync(string searchTerm)
    {
        var result = await _supabaseClient
            .From<Users>()
            .Where(u => u.user_name.Contains(searchTerm) || 
                       u.full_name.Contains(searchTerm) ||
                       u.email.Contains(searchTerm))
            .Get();

        return result.Models;
    }

    public async Task<bool> BlockUserAsync(Guid userId, Guid blockedUserId)
    {
        try
        {
            var blockedUser = new Blocked_users
            {
                user_id = userId,
                blocked_user_id = blockedUserId,
                created_at = DateTime.UtcNow
            };

            await _supabaseClient
                .From<Blocked_users>()
                .Insert(blockedUser);

            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> UnblockUserAsync(Guid userId, Guid blockedUserId)
    {
        try
        {
            await _supabaseClient
                .From<Blocked_users>()
                .Where(bu => bu.user_id == userId && bu.blocked_user_id == blockedUserId)
                .Delete();

            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<IEnumerable<Users>> GetBlockedUsersAsync(Guid userId)
    {
        var blockedUsers = await _supabaseClient
            .From<Blocked_users>()
            .Where(bu => bu.user_id == userId)
            .Get();

        var blockedUserIds = blockedUsers.Models.Select(bu => bu.blocked_user_id).ToList();
        
        if (!blockedUserIds.Any())
            return Enumerable.Empty<Users>();

        var users = await _supabaseClient
            .From<Users>()
            .Where(u => blockedUserIds.Contains(u.id))
            .Get();

        return users.Models;
    }

    public async Task<bool> IsUserBlockedAsync(Guid userId, Guid blockedUserId)
    {
        var result = await _supabaseClient
            .From<Blocked_users>()
            .Where(bu => bu.user_id == userId && bu.blocked_user_id == blockedUserId)
            .Get();

        return result.Models.Any();
    }
}