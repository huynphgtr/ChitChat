using ChitChatApp.Core.Application.Interfaces;
using ChitChatApp.Core.Domain.Entity;
using ChitChatApp.Core.Domain.Entity.Enums;
using ChitChatApp.Core.Infrastructure.Supabase;
using SupabaseClient = Supabase.Client;
using Supabase.Realtime;
using Supabase.Realtime.Models;

namespace ChitChatApp.Core.Application.Services;

public class ChatService : IChatService, IAsyncDisposable
{
    private readonly SupabaseClient _supabaseClient;
    private readonly IMessageRepository _messageRepository;
    private readonly IAuthService _authService;
    private RealtimeChannel? _channel;
    private Guid? _currentRoomId;

    public event Func<Messages, Task>? OnMessageReceived;
    public event Func<Messages, Task>? OnMessageUpdated;
    public event Func<long, Task>? OnMessageDeleted;

    public ChatService(
        SupabaseService supabaseService, 
        IMessageRepository messageRepository,
        IAuthService authService)
    {
        _supabaseClient = supabaseService.Client;
        _messageRepository = messageRepository;
        _authService = authService;
    }

    public async Task SubscribeToRoomMessagesAsync(Guid roomId)
    {
        // Unsubscribe from previous room if any
        if (_channel != null)
            await UnsubscribeFromRoomMessagesAsync();

        _currentRoomId = roomId;
        
        // Create new channel for this room
        _channel = _supabaseClient.Realtime.Channel($"room_{roomId}");
        
        // For now, we'll implement a simplified version without real-time events
        // Real-time functionality can be added later when the API is stable
        
        await Task.CompletedTask; // Placeholder for now
    }

    public async Task UnsubscribeFromRoomMessagesAsync()
    {
        if (_channel != null)
        {
            // Simplified unsubscribe for now
            _channel = null;
        }
        _currentRoomId = null;
        await Task.CompletedTask;
    }

    public async Task<bool> SendMessageAsync(Guid roomId, string content, MessageType messageType = MessageType.Text)
    {
        try
        {
            var currentUserId = _authService.GetCurrentUserId();
            if (!currentUserId.HasValue)
                return false;

            var message = new Messages(content, messageType.ToString())
            {
                room_id = roomId,
                sender_id = currentUserId.Value,
                sent_at = DateTime.UtcNow,
                is_read = false
            };

            await _messageRepository.SendMessageAsync(message);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<IEnumerable<Messages>> GetRoomMessagesAsync(Guid roomId, int page = 1, int pageSize = 50)
    {
        return await _messageRepository.GetMessagesForRoomAsync(roomId, page, pageSize);
    }

    public async Task<bool> MarkMessageAsReadAsync(long messageId)
    {
        var currentUserId = _authService.GetCurrentUserId();
        if (!currentUserId.HasValue)
            return false;

        return await _messageRepository.MarkMessageAsReadAsync(messageId, currentUserId.Value);
    }

    public async Task<int> GetUnreadCountAsync(Guid roomId, Guid userId)
    {
        return await _messageRepository.GetUnreadMessageCountForRoomAsync(roomId, userId);
    }

    // Real-time event handlers - placeholder implementations
    // These will be implemented properly when real-time functionality is added
    
    public async Task TriggerMessageReceived(Messages message)
    {
        if (OnMessageReceived != null)
        {
            await OnMessageReceived.Invoke(message);
        }
    }

    public async Task TriggerMessageUpdated(Messages message)
    {
        if (OnMessageUpdated != null)
        {
            await OnMessageUpdated.Invoke(message);
        }
    }

    public async Task TriggerMessageDeleted(long messageId)
    {
        if (OnMessageDeleted != null)
        {
            await OnMessageDeleted.Invoke(messageId);
        }
    }

    public async ValueTask DisposeAsync()
    {
        await UnsubscribeFromRoomMessagesAsync();
    }
}