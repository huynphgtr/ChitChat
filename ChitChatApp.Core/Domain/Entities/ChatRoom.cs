using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace ChitChatApp.Core.Domain.Entities;

[Table("chat_rooms")]
public class ChatRoom : BaseModel
{
    [PrimaryKey("id", false)]
    public Guid Id { get; set; }

    [Column("name")]
    public string? Name { get; set; }

    [Column("is_group")]
    public bool IsGroup { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; }

    [Column("is_deleted")]
    public bool IsDeleted { get; set; }
}