using ChitChatApp.Core.Domain.Entity;

namespace ChitChatApp.Core.Application.Interfaces;

public interface IMessageRepository
{
    Task<IEnumerable<Messages>> GetMessagesForRoomAsync(Guid roomId, int page = 1, int pageSize = 50);
    Task<Messages?> GetMessageByIdAsync(long messageId);
    Task<Messages> SendMessageAsync(Messages message);
    Task<Messages> UpdateMessageAsync(Messages message);
    Task<bool> DeleteMessageAsync(long messageId);
    Task<bool> MarkMessageAsReadAsync(long messageId, Guid userId);
    Task<IEnumerable<Messages>> GetUnreadMessagesForUserAsync(Guid userId);
    Task<int> GetUnreadMessageCountForRoomAsync(Guid roomId, Guid userId);
}