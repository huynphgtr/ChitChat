namespace ChitChatApp.Share.Services;

public interface INotificationService
{
    /// <summary>
    /// Shows a success notification
    /// </summary>
    /// <param name="message">The message to display</param>
    /// <param name="title">Optional title for the notification</param>
    /// <param name="duration">Duration in milliseconds, 0 for auto-dismiss</param>
    Task ShowSuccessAsync(string message, string? title = null, int duration = 3000);

    /// <summary>
    /// Shows an error notification
    /// </summary>
    /// <param name="message">The error message to display</param>
    /// <param name="title">Optional title for the notification</param>
    /// <param name="duration">Duration in milliseconds, 0 for no auto-dismiss</param>
    Task ShowErrorAsync(string message, string? title = null, int duration = 5000);

    /// <summary>
    /// Shows an information notification
    /// </summary>
    /// <param name="message">The information message to display</param>
    /// <param name="title">Optional title for the notification</param>
    /// <param name="duration">Duration in milliseconds, 0 for auto-dismiss</param>
    Task ShowInfoAsync(string message, string? title = null, int duration = 3000);

    /// <summary>
    /// Shows a warning notification
    /// </summary>
    /// <param name="message">The warning message to display</param>
    /// <param name="title">Optional title for the notification</param>
    /// <param name="duration">Duration in milliseconds, 0 for auto-dismiss</param>
    Task ShowWarningAsync(string message, string? title = null, int duration = 4000);

    /// <summary>
    /// Shows a custom notification with icon
    /// </summary>
    /// <param name="message">The message to display</param>
    /// <param name="title">Optional title for the notification</param>
    /// <param name="iconType">Type of icon to display</param>
    /// <param name="duration">Duration in milliseconds, 0 for auto-dismiss</param>
    Task ShowCustomAsync(string message, string? title = null, NotificationIcon iconType = NotificationIcon.Info, int duration = 3000);

    /// <summary>
    /// Clears all active notifications
    /// </summary>
    Task ClearAllAsync();
}

public enum NotificationIcon
{
    None,
    Info,
    Success,
    Warning,
    Error,
    Question
}

