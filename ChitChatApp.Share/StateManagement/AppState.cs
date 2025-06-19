using CommunityToolkit.Mvvm.ComponentModel;

namespace ChitChatApp.Share.StateManagement;

public partial class AppState : ObservableObject
{
    [ObservableProperty]
    private bool isAuthenticated;

    [ObservableProperty]
    private Guid? currentUserId;

    [ObservableProperty]
    private string? currentUserName;

    [ObservableProperty]
    private string? currentUserFullName;

    [ObservableProperty]
    private string? currentUserEmail;

    [ObservableProperty]
    private string? currentUserAvatar;

    [ObservableProperty]
    private bool isUserOnline = true;

    [ObservableProperty]
    private string? currentUserStatus;

    [ObservableProperty]
    private Guid? activeeChatRoomId;

    [ObservableProperty]
    private string? activeChatRoomName;

    [ObservableProperty]
    private bool isConnectedToRealtime;

    [ObservableProperty]
    private DateTime? lastSyncTime;

    [ObservableProperty]
    private bool isDarkTheme;

    [ObservableProperty]
    private string currentLanguage = "en";

    [ObservableProperty]
    private bool notificationsEnabled = true;

    [ObservableProperty]
    private bool soundEnabled = true;

    [ObservableProperty]
    private int unreadMessagesCount;

    [ObservableProperty]
    private int unreadChatsCount;

    public event EventHandler? AuthenticationStateChanged;
    public event EventHandler? UserProfileUpdated;
    public event EventHandler? ActiveChatChanged;
    public event EventHandler? ConnectionStateChanged;
    public event EventHandler? SettingsChanged;

    public string DisplayName => !string.IsNullOrEmpty(CurrentUserFullName) 
        ? CurrentUserFullName 
        : CurrentUserName ?? "Unknown User";

    public void Login(Guid userId, string userName, string? fullName = null, string? email = null, string? avatarUrl = null)
    {
        CurrentUserId = userId;
        CurrentUserName = userName;
        CurrentUserFullName = fullName;
        CurrentUserEmail = email;
        CurrentUserAvatar = avatarUrl;
        IsAuthenticated = true;
        
        AuthenticationStateChanged?.Invoke(this, EventArgs.Empty);
        UserProfileUpdated?.Invoke(this, EventArgs.Empty);
    }

    public void Logout()
    {
        CurrentUserId = null;
        CurrentUserName = null;
        CurrentUserFullName = null;
        CurrentUserEmail = null;
        CurrentUserAvatar = null;
        CurrentUserStatus = null;
        IsAuthenticated = false;
        IsUserOnline = false;
        ActiveeChatRoomId = null;
        ActiveChatRoomName = null;
        IsConnectedToRealtime = false;
        UnreadMessagesCount = 0;
        UnreadChatsCount = 0;
        
        AuthenticationStateChanged?.Invoke(this, EventArgs.Empty);
    }

    public void UpdateUserProfile(string? fullName = null, string? email = null, string? avatarUrl = null, string? status = null)
    {
        if (fullName != null)
            CurrentUserFullName = fullName;
        
        if (email != null)
            CurrentUserEmail = email;
        
        if (avatarUrl != null)
            CurrentUserAvatar = avatarUrl;
        
        if (status != null)
            CurrentUserStatus = status;
        
        UserProfileUpdated?.Invoke(this, EventArgs.Empty);
        OnPropertyChanged(nameof(DisplayName));
    }

    public void SetActiveChat(Guid? chatRoomId, string? chatRoomName = null)
    {
        ActiveeChatRoomId = chatRoomId;
        ActiveChatRoomName = chatRoomName;
        
        ActiveChatChanged?.Invoke(this, EventArgs.Empty);
    }

    public void UpdateConnectionState(bool isConnected)
    {
        IsConnectedToRealtime = isConnected;
        LastSyncTime = isConnected ? DateTime.Now : null;
        
        ConnectionStateChanged?.Invoke(this, EventArgs.Empty);
    }

    public void UpdateOnlineStatus(bool isOnline)
    {
        IsUserOnline = isOnline;
        UserProfileUpdated?.Invoke(this, EventArgs.Empty);
    }

    public void UpdateUnreadCounts(int messagesCount, int chatsCount)
    {
        UnreadMessagesCount = messagesCount;
        UnreadChatsCount = chatsCount;
    }

    public void IncrementUnreadCount(bool isNewChat = false)
    {
        UnreadMessagesCount++;
        if (isNewChat)
        {
            UnreadChatsCount++;
        }
    }

    public void DecrementUnreadCount(int messageCount, bool isChatRead = false)
    {
        UnreadMessagesCount = Math.Max(0, UnreadMessagesCount - messageCount);
        if (isChatRead)
        {
            UnreadChatsCount = Math.Max(0, UnreadChatsCount - 1);
        }
    }

    public void UpdateSettings(bool? isDarkTheme = null, string? language = null, bool? notificationsEnabled = null, bool? soundEnabled = null)
    {
        var settingsChanged = false;

        if (isDarkTheme.HasValue && IsDarkTheme != isDarkTheme.Value)
        {
            IsDarkTheme = isDarkTheme.Value;
            settingsChanged = true;
        }

        if (!string.IsNullOrEmpty(language) && CurrentLanguage != language)
        {
            CurrentLanguage = language;
            settingsChanged = true;
        }

        if (notificationsEnabled.HasValue && NotificationsEnabled != notificationsEnabled.Value)
        {
            NotificationsEnabled = notificationsEnabled.Value;
            settingsChanged = true;
        }

        if (soundEnabled.HasValue && SoundEnabled != soundEnabled.Value)
        {
            SoundEnabled = soundEnabled.Value;
            settingsChanged = true;
        }

        if (settingsChanged)
        {
            SettingsChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    public void Reset()
    {
        Logout();
        IsDarkTheme = false;
        CurrentLanguage = "en";
        NotificationsEnabled = true;
        SoundEnabled = true;
        LastSyncTime = null;
    }

    partial void OnCurrentUserFullNameChanged(string? value)
    {
        OnPropertyChanged(nameof(DisplayName));
    }

    partial void OnCurrentUserNameChanged(string? value)
    {
        OnPropertyChanged(nameof(DisplayName));
    }
}

