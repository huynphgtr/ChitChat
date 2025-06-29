using ChitChatApp.Core.Application.Interfaces;
using ChitChatApp.Core.Domain.Entity;
using ChitChatApp.Core.Infrastructure.Supabase;
using SupabaseClient = Supabase.Client;

namespace ChitChatApp.Core.Infrastructure.Persistence.Repositories;

public class ChatRoomRepository : IChatRoomRepository
{
    private readonly SupabaseClient _supabaseClient;

    public ChatRoomRepository(SupabaseService supabaseService)
    {
        _supabaseClient = supabaseService.Client;
    }

    public async Task<IEnumerable<Chat_rooms>> GetChatRoomsForUserAsync(Guid userId)
    {
        var result = await _supabaseClient
            .From<Room_participants>()
            .Select("*, chat_rooms(*)")
            .Where(rp => rp.user_id == userId)
            .Get();

        var roomIds = result.Models
            .Select(rp => rp.room_id)
            .ToList();

        if (!roomIds.Any())
            return Enumerable.Empty<Chat_rooms>();

        var rooms = await _supabaseClient
            .From<Chat_rooms>()
            .Where(cr => roomIds.Contains(cr.id))
            .Get();

        return rooms.Models;
    }

    public async Task<Chat_rooms?> GetChatRoomByIdAsync(Guid roomId)
    {
        var result = await _supabaseClient
            .From<Chat_rooms>()
            .Where(cr => cr.id == roomId)
            .Single();
        
        return result;
    }

    public async Task<Chat_rooms> CreateChatRoomAsync(Chat_rooms chatRoom)
    {
        var result = await _supabaseClient
            .From<Chat_rooms>()
            .Insert(chatRoom);
        
        return result.Models.First();
    }

    public async Task<Chat_rooms> UpdateChatRoomAsync(Chat_rooms chatRoom)
    {
        var result = await _supabaseClient
            .From<Chat_rooms>()
            .Update(chatRoom);
        
        return result.Models.First();
    }

    public async Task<bool> DeleteChatRoomAsync(Guid roomId)
    {
        try
        {
            await _supabaseClient
                .From<Chat_rooms>()
                .Where(cr => cr.id == roomId)
                .Delete();
            
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> AddParticipantAsync(Guid roomId, Guid userId)
    {
        try
        {
            var participant = new Room_participants
            {
                room_id = roomId,
                user_id = userId,
                joined_at = DateTime.UtcNow
            };

            await _supabaseClient
                .From<Room_participants>()
                .Insert(participant);
            
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> RemoveParticipantAsync(Guid roomId, Guid userId)
    {
        try
        {
            await _supabaseClient
                .From<Room_participants>()
                .Where(rp => rp.room_id == roomId && rp.user_id == userId)
                .Delete();
            
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<IEnumerable<Users>> GetRoomParticipantsAsync(Guid roomId)
    {
        var participantIds = await _supabaseClient
            .From<Room_participants>()
            .Where(rp => rp.room_id == roomId)
            .Get();

        var userIds = participantIds.Models
            .Select(rp => rp.user_id)
            .ToList();

        if (!userIds.Any())
            return Enumerable.Empty<Users>();

        var users = await _supabaseClient
            .From<Users>()
            .Where(u => userIds.Contains(u.id))
            .Get();

        return users.Models;
    }

    public async Task<bool> IsUserInRoomAsync(Guid roomId, Guid userId)
    {
        var result = await _supabaseClient
            .From<Room_participants>()
            .Where(rp => rp.room_id == roomId && rp.user_id == userId)
            .Get();
        
        return result.Models.Any();
    }
}