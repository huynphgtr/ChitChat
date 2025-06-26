using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace ChitChatApp.Core.Domain.Entities;

[Table("bot")]
public class Bot : BaseModel
{
    [PrimaryKey("id")]
    public int Id { get; set; }

    [Column("bot_name")]
    public string BotName { get; set; } = string.Empty;

    [Column("user_id")]
    public Guid UserId { get; set; }

    [Column("bot_api")]
    public string BotApi { get; set; } = string.Empty;

    [Column("created_at")]
    public DateTime CreatedAt { get; set; }
}