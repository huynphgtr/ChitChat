using ChitChatApp.Core.Domain.Entity;

namespace ChitChatApp.Core.Application.Interfaces;

public interface IChatRoomRepository
{
    Task<IEnumerable<Chat_rooms>> GetChatRoomsForUserAsync(Guid userId);
    Task<Chat_rooms?> GetChatRoomByIdAsync(Guid roomId);
    Task<Chat_rooms> CreateChatRoomAsync(Chat_rooms chatRoom);
    Task<Chat_rooms> UpdateChatRoomAsync(Chat_rooms chatRoom);
    Task<bool> DeleteChatRoomAsync(Guid roomId);
    Task<bool> AddParticipantAsync(Guid roomId, Guid userId);
    Task<bool> RemoveParticipantAsync(Guid roomId, Guid userId);
    Task<IEnumerable<Users>> GetRoomParticipantsAsync(Guid roomId);
    Task<bool> IsUserInRoomAsync(Guid roomId, Guid userId);
}