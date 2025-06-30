using ChitChatApp.Core.Domain.Entity;
using ChitChatApp.Core.Domain.Entity.Enums;

namespace ChitChatApp.Core.Application.Interfaces;

public interface IChatService
{
    event Func<Messages, Task>? OnMessageReceived;
    event Func<Messages, Task>? OnMessageUpdated;
    event Func<long, Task>? OnMessageDeleted;
    
    Task SubscribeToRoomMessagesAsync(Guid roomId);
    Task UnsubscribeFromRoomMessagesAsync();
    Task<bool> SendMessageAsync(Guid roomId, string content, MessageType messageType = MessageType.Text);
    Task<IEnumerable<Messages>> GetRoomMessagesAsync(Guid roomId, int page = 1, int pageSize = 50);
    Task<bool> MarkMessageAsReadAsync(long messageId);
    Task<int> GetUnreadCountAsync(Guid roomId, Guid userId);
}