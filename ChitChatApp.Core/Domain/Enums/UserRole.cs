namespace ChitChatApp.Core.Domain.Enums;

public enum UserRole
{
    Admin,
    Member,
    None
}

public static class UserRoleExtensions
{
    public static string ToDbValue(this UserRole role)
    {
        return role.ToString().ToLowerInvariant();
    }

    public static UserRole FromDbValue(string value)
    {
        return value.ToLowerInvariant() switch
        {
            "admin" => UserRole.Admin,
            "member" => UserRole.Member,
            "none" => UserRole.None,
            _ => UserRole.None
        };
    }
}