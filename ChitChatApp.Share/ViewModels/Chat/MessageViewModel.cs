using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ChitChatApp.Share.ViewModels.Base;

namespace ChitChatApp.Share.ViewModels.Chat;

public partial class MessageViewModel : BaseViewModel
{
    [ObservableProperty]
    private long messageId;

    [ObservableProperty]
    private Guid senderId;

    [ObservableProperty]
    private string senderName = string.Empty;

    [ObservableProperty]
    private string senderAvatar = string.Empty;

    [ObservableProperty]
    private string content = string.Empty;

    [ObservableProperty]
    private string messageType = "text"; // text, image, file, system

    [ObservableProperty]
    private DateTime sentAt;

    [ObservableProperty]
    private bool isFromCurrentUser;

    [ObservableProperty]
    private bool isRead;

    [ObservableProperty]
    private bool isDelivered = true;

    [ObservableProperty]
    private bool isEdited;

    [ObservableProperty]
    private DateTime? editedAt;

    [ObservableProperty]
    private string? attachmentUrl;

    [ObservableProperty]
    private string? attachmentFileName;

    [ObservableProperty]
    private long? attachmentFileSize;

    [ObservableProperty]
    private bool isSelected;

    [ObservableProperty]
    private bool canEdit;

    [ObservableProperty]
    private bool canDelete;

    public event EventHandler<MessageViewModel>? MessageReply;
    public event EventHandler<MessageViewModel>? MessageEdit;
    public event EventHandler<MessageViewModel>? MessageDelete;
    public event EventHandler<MessageViewModel>? MessageForward;
    public event EventHandler<string>? AttachmentDownload;

    public string FormattedTime => SentAt.ToString("HH:mm");
    public string FormattedDate => SentAt.ToString("dd/MM/yyyy");
    public string FormattedDateTime => SentAt.ToString("dd/MM/yyyy HH:mm");

    public bool IsToday => SentAt.Date == DateTime.Today;
    public bool IsYesterday => SentAt.Date == DateTime.Today.AddDays(-1);

    public string RelativeTime
    {
        get
        {
            var timeSpan = DateTime.Now - SentAt;
            
            if (timeSpan.TotalMinutes < 1)
                return "Just now";
            if (timeSpan.TotalMinutes < 60)
                return $"{(int)timeSpan.TotalMinutes}m ago";
            if (timeSpan.TotalHours < 24)
                return $"{(int)timeSpan.TotalHours}h ago";
            if (timeSpan.TotalDays < 7)
                return $"{(int)timeSpan.TotalDays}d ago";
            
            return FormattedDate;
        }
    }

    public bool HasAttachment => !string.IsNullOrEmpty(AttachmentUrl);

    public string AttachmentSizeFormatted
    {
        get
        {
            if (!AttachmentFileSize.HasValue)
                return string.Empty;

            var size = AttachmentFileSize.Value;
            string[] sizes = { "B", "KB", "MB", "GB" };
            int order = 0;
            while (size >= 1024 && order < sizes.Length - 1)
            {
                order++;
                size = size / 1024;
            }
            return $"{size:0.##} {sizes[order]}";
        }
    }

    public MessageViewModel()
    {
        UpdatePermissions();
    }

    [RelayCommand]
    private void Reply()
    {
        MessageReply?.Invoke(this, this);
    }

    [RelayCommand]
    private void Edit()
    {
        if (CanEdit)
        {
            MessageEdit?.Invoke(this, this);
        }
    }

    [RelayCommand]
    private void Delete()
    {
        if (CanDelete)
        {
            MessageDelete?.Invoke(this, this);
        }
    }

    [RelayCommand]
    private void Forward()
    {
        MessageForward?.Invoke(this, this);
    }

    [RelayCommand]
    private void DownloadAttachment()
    {
        if (HasAttachment && !string.IsNullOrEmpty(AttachmentUrl))
        {
            AttachmentDownload?.Invoke(this, AttachmentUrl);
        }
    }

    [RelayCommand]
    private void CopyText()
    {
        if (!string.IsNullOrEmpty(Content))
        {
            // This would typically copy to clipboard
            // Clipboard.SetText(Content);
        }
    }

    public void MarkAsRead()
    {
        IsRead = true;
    }

    public void MarkAsDelivered()
    {
        IsDelivered = true;
    }

    public void UpdateEditedContent(string newContent)
    {
        Content = newContent;
        IsEdited = true;
        EditedAt = DateTime.Now;
    }

    private void UpdatePermissions()
    {
        // Users can typically edit/delete their own messages within a certain time frame
        var timeSinceMessage = DateTime.Now - SentAt;
        CanEdit = IsFromCurrentUser && MessageType == "text" && timeSinceMessage.TotalMinutes < 15;
        CanDelete = IsFromCurrentUser && timeSinceMessage.TotalHours < 24;
    }

    partial void OnIsFromCurrentUserChanged(bool value)
    {
        UpdatePermissions();
    }

    partial void OnSentAtChanged(DateTime value)
    {
        UpdatePermissions();
        OnPropertyChanged(nameof(FormattedTime));
        OnPropertyChanged(nameof(FormattedDate));
        OnPropertyChanged(nameof(FormattedDateTime));
        OnPropertyChanged(nameof(IsToday));
        OnPropertyChanged(nameof(IsYesterday));
        OnPropertyChanged(nameof(RelativeTime));
    }

    partial void OnAttachmentFileSizeChanged(long? value)
    {
        OnPropertyChanged(nameof(AttachmentSizeFormatted));
    }

    partial void OnAttachmentUrlChanged(string? value)
    {
        OnPropertyChanged(nameof(HasAttachment));
    }
}

