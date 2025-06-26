namespace ChitChatApp.Core.Domain.Enums;

public enum MessageType
{
    Text,
    Image,
    File,
    System
}

public static class MessageTypeExtensions
{
    public static string ToDbValue(this MessageType type)
    {
        return type.ToString().ToLowerInvariant();
    }

    public static MessageType FromDbValue(string value)
    {
        return value.ToLowerInvariant() switch
        {
            "text" => MessageType.Text,
            "image" => MessageType.Image,
            "file" => MessageType.File,
            "system" => MessageType.System,
            _ => MessageType.Text
        };
    }
}