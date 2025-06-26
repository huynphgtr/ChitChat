using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace ChitChatApp.Core.Domain.Entities;

[Table("room_participants")]
public class RoomParticipant : BaseModel
{
    [PrimaryKey("id")]
    public long Id { get; set; }

    [Column("room_id")]
    public Guid RoomId { get; set; }

    [Column("user_id")]
    public Guid UserId { get; set; }

    [Column("joined_at")]
    public DateTime JoinedAt { get; set; }
}