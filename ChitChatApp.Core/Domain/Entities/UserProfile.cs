using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace ChitChatApp.Core.Domain.Entities;

[Table("users")]
public class UserProfile : BaseModel
{
    [PrimaryKey("id", false)]
    public Guid Id { get; set; }

    [Column("email")]
    public string Email { get; set; } = string.Empty;

    [Column("full_name")]
    public string? FullName { get; set; }

    [Column("user_name")]
    public string? UserName { get; set; }

    [Column("avatar_url")]
    public string? AvatarUrl { get; set; }

    [Column("user_role")]
    public string UserRole { get; set; } = "user";

    [Column("status")]
    public bool Status { get; set; } = true;
}