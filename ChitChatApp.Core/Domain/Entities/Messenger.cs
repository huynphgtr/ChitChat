using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace ChitChatApp.Core.Domain.Entities;

[Table("messages")]
public class Message : BaseModel
{
    [PrimaryKey("id", false)]
    public long Id { get; set; }

    [Column("room_id")]
    public Guid RoomId { get; set; }

    [Column("sender_id")]
    public Guid SenderId { get; set; }

    [Column("content")]
    public string Content { get; set; } = string.Empty;

    [Column("message_type")]
    public string MessageType { get; set; } = "text";

    [Column("sent_at")]
    public DateTime SentAt { get; set; }

    [Column("is_read")]
    public bool IsRead { get; set; }
}