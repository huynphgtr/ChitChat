using ChitChatApp.Core.Application.Interfaces;
using ChitChatApp.Core.Domain.Entity;
using ChitChatApp.Core.Domain.Entity.Enums;
using ChitChatApp.Core.Infrastructure.Supabase;
using SupabaseClient = Supabase.Client;
using static Supabase.Postgrest.Constants;

namespace ChitChatApp.Core.Infrastructure.Persistence.Repositories;

public class MessageRepository : IMessageRepository
{
    private readonly SupabaseClient _supabaseClient;

    public MessageRepository(SupabaseService supabaseService)
    {
        _supabaseClient = supabaseService.Client;
    }

    public async Task<IEnumerable<Messages>> GetMessagesForRoomAsync(Guid roomId, int page = 1, int pageSize = 50)
    {
        var offset = (page - 1) * pageSize;
        
        var result = await _supabaseClient
            .From<Messages>()
            .Where(m => m.room_id == roomId)
            .Order("sent_at", Ordering.Descending)
            .Range(offset, offset + pageSize - 1)
            .Get();
        
        return result.Models.OrderBy(m => m.sent_at).ToList(); // Return in chronological order
    }

    public async Task<Messages?> GetMessageByIdAsync(long messageId)
    {
        var result = await _supabaseClient
            .From<Messages>()
            .Where(m => m.id == messageId)
            .Single();
        
        return result;
    }

    public async Task<Messages> SendMessageAsync(Messages message)
    {
        message.sent_at = DateTime.UtcNow;
        message.is_read = false;

        var result = await _supabaseClient
            .From<Messages>()
            .Insert(message);
        
        return result.Models.First();
    }

    public async Task<Messages> UpdateMessageAsync(Messages message)
    {
        var result = await _supabaseClient
            .From<Messages>()
            .Update(message);
        
        return result.Models.First();
    }

    public async Task<bool> DeleteMessageAsync(long messageId)
    {
        try
        {
            await _supabaseClient
                .From<Messages>()
                .Where(m => m.id == messageId)
                .Delete();
            
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> MarkMessageAsReadAsync(long messageId, Guid userId)
    {
        try
        {
            var message = await GetMessageByIdAsync(messageId);
            if (message == null) return false;

            // Update message as read
            message.is_read = true;
            await UpdateMessageAsync(message);

            // Update or create message status
            var existingStatus = await _supabaseClient
                .From<Message_status>()
                .Where(ms => ms.message_id == messageId && ms.receiver_id == userId)
                .Single();

            if (existingStatus != null)
            {
                existingStatus.status = MessageStatus.Read;
                existingStatus.updated_at = DateTime.UtcNow;
                await _supabaseClient
                    .From<Message_status>()
                    .Update(existingStatus);
            }
            else
            {
                var messageStatus = new Message_status
                {
                    message_id = messageId,
                    receiver_id = userId,
                    status = MessageStatus.Read,
                    updated_at = DateTime.UtcNow
                };

                await _supabaseClient
                    .From<Message_status>()
                    .Insert(messageStatus);
            }

            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<IEnumerable<Messages>> GetUnreadMessagesForUserAsync(Guid userId)
    {
        var result = await _supabaseClient
            .From<Messages>()
            .Where(m => m.sender_id != userId && m.is_read == false)
            .Get();
        
        return result.Models;
    }

    public async Task<int> GetUnreadMessageCountForRoomAsync(Guid roomId, Guid userId)
    {
        var result = await _supabaseClient
            .From<Messages>()
            .Where(m => m.room_id == roomId && m.sender_id != userId && m.is_read == false)
            .Get();
        
        return result.Models.Count();
    }
}