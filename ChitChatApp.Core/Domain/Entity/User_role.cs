using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;
using ChitChatApp.Core.Domain.Entity.Enums;

namespace ChitChatApp.Core.Domain.Entity;

[Table("user_role")]
public class User_role : BaseModel
{
    [PrimaryKey("id", true)]
    public int id { get; set; }
    
    [Column("user_id")]
    public Guid? user_id { get; set; }
    
    [Column("chat_rooms_id")]
    public Guid? chat_rooms_id { get; set; }
    
    [Column("role")]
    public UserRole? role { get; set; }
}