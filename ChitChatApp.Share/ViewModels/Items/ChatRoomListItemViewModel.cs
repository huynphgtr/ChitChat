using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ChitChatApp.Share.ViewModels.Base;

namespace ChitChatApp.Share.ViewModels.Items;

public partial class ChatRoomListItemViewModel : BaseViewModel
{
    [ObservableProperty]
    private Guid chatRoomId;

    [ObservableProperty]
    private string name = string.Empty;

    [ObservableProperty]
    private bool isGroup;

    [ObservableProperty]
    private string? avatarUrl;

    [ObservableProperty]
    private string? lastMessage;

    [ObservableProperty]
    private string? lastMessageSender;

    [ObservableProperty]
    private DateTime lastMessageTime;

    [ObservableProperty]
    private int unreadCount;

    [ObservableProperty]
    private bool isPinned;

    [ObservableProperty]
    private bool isMuted;

    [ObservableProperty]
    private bool isArchived;

    [ObservableProperty]
    private bool isSelected;

    [ObservableProperty]
    private int participantCount;

    [ObservableProperty]
    private bool isTyping;

    [ObservableProperty]
    private string? typingUserName;

    [ObservableProperty]
    private DateTime createdAt;

    public event EventHandler<ChatRoomListItemViewModel>? ChatRoomSelected;
    public event EventHandler<ChatRoomListItemViewModel>? PinToggled;
    public event EventHandler<ChatRoomListItemViewModel>? MuteToggled;
    public event EventHandler<ChatRoomListItemViewModel>? ArchiveToggled;
    public event EventHandler<ChatRoomListItemViewModel>? LeaveRequested;
    public event EventHandler<ChatRoomListItemViewModel>? DeleteRequested;

    public string DisplayName => Name;

    public string LastMessageDisplay
    {
        get
        {
            if (IsTyping && !string.IsNullOrEmpty(TypingUserName))
            {
                return IsGroup ? $"{TypingUserName} is typing..." : "Typing...";
            }

            if (string.IsNullOrEmpty(LastMessage))
                return "No messages yet";

            if (IsGroup && !string.IsNullOrEmpty(LastMessageSender))
            {
                return $"{LastMessageSender}: {LastMessage}";
            }

            return LastMessage;
        }
    }

    public string LastMessageTimeDisplay
    {
        get
        {
            if (LastMessageTime == default)
                return string.Empty;

            var timeSpan = DateTime.Now - LastMessageTime;

            if (timeSpan.TotalMinutes < 1)
                return "Now";
            if (timeSpan.TotalMinutes < 60)
                return $"{(int)timeSpan.TotalMinutes}m";
            if (timeSpan.TotalHours < 24)
                return $"{(int)timeSpan.TotalHours}h";
            if (timeSpan.TotalDays < 7)
                return $"{(int)timeSpan.TotalDays}d";

            return LastMessageTime.ToString("dd/MM");
        }
    }

    public bool HasUnreadMessages => UnreadCount > 0;

    public string UnreadCountDisplay => UnreadCount > 99 ? "99+" : UnreadCount.ToString();

    public string InitialsText
    {
        get
        {
            if (string.IsNullOrEmpty(Name))
                return "?";

            var parts = Name.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 1)
                return parts[0].Substring(0, Math.Min(2, parts[0].Length)).ToUpper();

            return $"{parts[0][0]}{parts[^1][0]}".ToUpper();
        }
    }

    public string ParticipantCountDisplay => IsGroup ? $"{ParticipantCount} members" : string.Empty;

    [RelayCommand]
    private void Select()
    {
        IsSelected = true;
        ChatRoomSelected?.Invoke(this, this);
    }

    [RelayCommand]
    private void TogglePin()
    {
        IsPinned = !IsPinned;
        PinToggled?.Invoke(this, this);
    }

    [RelayCommand]
    private void ToggleMute()
    {
        IsMuted = !IsMuted;
        MuteToggled?.Invoke(this, this);
    }

    [RelayCommand]
    private void ToggleArchive()
    {
        IsArchived = !IsArchived;
        ArchiveToggled?.Invoke(this, this);
    }

    [RelayCommand]
    private void Leave()
    {
        LeaveRequested?.Invoke(this, this);
    }

    [RelayCommand]
    private void Delete()
    {
        DeleteRequested?.Invoke(this, this);
    }

    public void UpdateLastMessage(string message, string? senderName, DateTime timestamp)
    {
        LastMessage = message;
        LastMessageSender = senderName;
        LastMessageTime = timestamp;
        
        if (!IsSelected && !IsMuted)
        {
            UnreadCount++;
        }

        OnPropertyChanged(nameof(LastMessageDisplay));
        OnPropertyChanged(nameof(LastMessageTimeDisplay));
        OnPropertyChanged(nameof(HasUnreadMessages));
        OnPropertyChanged(nameof(UnreadCountDisplay));
    }

    public void MarkAsRead()
    {
        UnreadCount = 0;
        OnPropertyChanged(nameof(HasUnreadMessages));
        OnPropertyChanged(nameof(UnreadCountDisplay));
    }

    public void UpdateTypingStatus(bool isTyping, string? userName = null)
    {
        IsTyping = isTyping;
        TypingUserName = userName;
        OnPropertyChanged(nameof(LastMessageDisplay));
    }

    public void UpdateParticipantCount(int count)
    {
        ParticipantCount = count;
        OnPropertyChanged(nameof(ParticipantCountDisplay));
    }

    partial void OnNameChanged(string value)
    {
        OnPropertyChanged(nameof(DisplayName));
        OnPropertyChanged(nameof(InitialsText));
    }

    partial void OnLastMessageChanged(string? value)
    {
        OnPropertyChanged(nameof(LastMessageDisplay));
    }

    partial void OnLastMessageSenderChanged(string? value)
    {
        OnPropertyChanged(nameof(LastMessageDisplay));
    }

    partial void OnLastMessageTimeChanged(DateTime value)
    {
        OnPropertyChanged(nameof(LastMessageTimeDisplay));
    }

    partial void OnUnreadCountChanged(int value)
    {
        OnPropertyChanged(nameof(HasUnreadMessages));
        OnPropertyChanged(nameof(UnreadCountDisplay));
    }

    partial void OnIsTypingChanged(bool value)
    {
        OnPropertyChanged(nameof(LastMessageDisplay));
    }

    partial void OnTypingUserNameChanged(string? value)
    {
        OnPropertyChanged(nameof(LastMessageDisplay));
    }

    partial void OnParticipantCountChanged(int value)
    {
        OnPropertyChanged(nameof(ParticipantCountDisplay));
    }

    partial void OnIsGroupChanged(bool value)
    {
        OnPropertyChanged(nameof(ParticipantCountDisplay));
    }
}

