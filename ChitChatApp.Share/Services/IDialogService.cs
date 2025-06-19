namespace ChitChatApp.Share.Services;

public interface IDialogService
{
    /// <summary>
    /// Shows a confirmation dialog with Yes/No buttons
    /// </summary>
    /// <param name="message">The message to display</param>
    /// <param name="title">Optional title for the dialog</param>
    /// <param name="confirmText">Text for the confirm button (default: "Yes")</param>
    /// <param name="cancelText">Text for the cancel button (default: "No")</param>
    /// <returns>True if user confirmed, false otherwise</returns>
    Task<bool> ShowConfirmationAsync(string message, string? title = null, string confirmText = "Yes", string cancelText = "No");

    /// <summary>
    /// Shows an alert dialog with OK button
    /// </summary>
    /// <param name="message">The message to display</param>
    /// <param name="title">Optional title for the dialog</param>
    /// <param name="buttonText">Text for the OK button (default: "OK")</param>
    Task ShowAlertAsync(string message, string? title = null, string buttonText = "OK");

    /// <summary>
    /// Shows an error dialog
    /// </summary>
    /// <param name="message">The error message to display</param>
    /// <param name="title">Optional title for the dialog (default: "Error")</param>
    /// <param name="buttonText">Text for the OK button (default: "OK")</param>
    Task ShowErrorAsync(string message, string? title = "Error", string buttonText = "OK");

    /// <summary>
    /// Shows a prompt dialog for text input
    /// </summary>
    /// <param name="message">The message to display</param>
    /// <param name="title">Optional title for the dialog</param>
    /// <param name="defaultValue">Default value for the input field</param>
    /// <param name="placeholder">Placeholder text for the input field</param>
    /// <param name="confirmText">Text for the confirm button (default: "OK")</param>
    /// <param name="cancelText">Text for the cancel button (default: "Cancel")</param>
    /// <returns>The entered text if confirmed, null if cancelled</returns>
    Task<string?> ShowPromptAsync(string message, string? title = null, string? defaultValue = null, string? placeholder = null, string confirmText = "OK", string cancelText = "Cancel");

    /// <summary>
    /// Shows a dialog for selecting from multiple options
    /// </summary>
    /// <param name="message">The message to display</param>
    /// <param name="title">Optional title for the dialog</param>
    /// <param name="options">List of options to choose from</param>
    /// <param name="cancelText">Text for the cancel button (default: "Cancel")</param>
    /// <returns>The selected option index, or -1 if cancelled</returns>
    Task<int> ShowOptionsAsync(string message, string? title, IList<string> options, string cancelText = "Cancel");

    /// <summary>
    /// Shows a file picker dialog
    /// </summary>
    /// <param name="title">Title for the file picker</param>
    /// <param name="fileTypes">Allowed file types (e.g., "*.jpg;*.png")</param>
    /// <param name="multiSelect">Allow multiple file selection</param>
    /// <returns>Selected file paths, empty if cancelled</returns>
    Task<IList<string>> ShowFilePickerAsync(string? title = null, string? fileTypes = null, bool multiSelect = false);

    /// <summary>
    /// Shows a folder picker dialog
    /// </summary>
    /// <param name="title">Title for the folder picker</param>
    /// <returns>Selected folder path, null if cancelled</returns>
    Task<string?> ShowFolderPickerAsync(string? title = null);

    /// <summary>
    /// Shows a save file dialog
    /// </summary>
    /// <param name="title">Title for the save dialog</param>
    /// <param name="defaultFileName">Default file name</param>
    /// <param name="fileTypes">Allowed file types (e.g., "*.txt")</param>
    /// <returns>Selected file path, null if cancelled</returns>
    Task<string?> ShowSaveFileDialogAsync(string? title = null, string? defaultFileName = null, string? fileTypes = null);

    /// <summary>
    /// Shows a loading dialog with optional progress
    /// </summary>
    /// <param name="message">The loading message</param>
    /// <param name="title">Optional title for the dialog</param>
    /// <param name="showProgress">Whether to show progress bar</param>
    /// <returns>A disposable that closes the dialog when disposed</returns>
    IDisposable ShowLoading(string message, string? title = null, bool showProgress = false);

    /// <summary>
    /// Updates the progress of a loading dialog
    /// </summary>
    /// <param name="progress">Progress value between 0 and 100</param>
    /// <param name="message">Optional message to update</param>
    void UpdateLoadingProgress(double progress, string? message = null);
}

