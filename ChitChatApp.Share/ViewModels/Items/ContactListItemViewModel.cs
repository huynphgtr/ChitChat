using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ChitChatApp.Share.ViewModels.Base;

namespace ChitChatApp.Share.ViewModels.Items;

public partial class ContactListItemViewModel : BaseViewModel
{
    [ObservableProperty]
    private Guid userId;

    [ObservableProperty]
    private string userName = string.Empty;

    [ObservableProperty]
    private string fullName = string.Empty;

    [ObservableProperty]
    private string email = string.Empty;

    [ObservableProperty]
    private string? avatarUrl;

    [ObservableProperty]
    private bool isOnline;

    [ObservableProperty]
    private DateTime lastSeen;

    [ObservableProperty]
    private string status = string.Empty;

    [ObservableProperty]
    private bool isBlocked;

    [ObservableProperty]
    private bool isFavorite;

    [ObservableProperty]
    private bool isSelected;

    [ObservableProperty]
    private DateTime addedAt;

    public event EventHandler<ContactListItemViewModel>? ContactSelected;
    public event EventHandler<ContactListItemViewModel>? StartChatRequested;
    public event EventHandler<ContactListItemViewModel>? BlockRequested;
    public event EventHandler<ContactListItemViewModel>? UnblockRequested;
    public event EventHandler<ContactListItemViewModel>? RemoveRequested;
    public event EventHandler<ContactListItemViewModel>? FavoriteToggled;

    public string DisplayName => !string.IsNullOrEmpty(FullName) ? FullName : UserName;

    public string OnlineStatusText
    {
        get
        {
            if (IsOnline)
                return "Online";
            
            var timeSinceLastSeen = DateTime.Now - LastSeen;
            
            if (timeSinceLastSeen.TotalMinutes < 1)
                return "Just now";
            if (timeSinceLastSeen.TotalMinutes < 60)
                return $"Last seen {(int)timeSinceLastSeen.TotalMinutes}m ago";
            if (timeSinceLastSeen.TotalHours < 24)
                return $"Last seen {(int)timeSinceLastSeen.TotalHours}h ago";
            if (timeSinceLastSeen.TotalDays < 7)
                return $"Last seen {(int)timeSinceLastSeen.TotalDays}d ago";
            
            return $"Last seen {LastSeen:dd/MM/yyyy}";
        }
    }

    public string InitialsText
    {
        get
        {
            var name = DisplayName;
            if (string.IsNullOrEmpty(name))
                return "?";

            var parts = name.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 1)
                return parts[0].Substring(0, Math.Min(2, parts[0].Length)).ToUpper();
            
            return $"{parts[0][0]}{parts[^1][0]}".ToUpper();
        }
    }

    [RelayCommand]
    private void Select()
    {
        IsSelected = true;
        ContactSelected?.Invoke(this, this);
    }

    [RelayCommand]
    private void StartChat()
    {
        StartChatRequested?.Invoke(this, this);
    }

    [RelayCommand]
    private void Block()
    {
        if (!IsBlocked)
        {
            BlockRequested?.Invoke(this, this);
        }
    }

    [RelayCommand]
    private void Unblock()
    {
        if (IsBlocked)
        {
            UnblockRequested?.Invoke(this, this);
        }
    }

    [RelayCommand]
    private void Remove()
    {
        RemoveRequested?.Invoke(this, this);
    }

    [RelayCommand]
    private void ToggleFavorite()
    {
        IsFavorite = !IsFavorite;
        FavoriteToggled?.Invoke(this, this);
    }

    public void UpdateOnlineStatus(bool isOnline, DateTime? lastSeen = null)
    {
        IsOnline = isOnline;
        if (lastSeen.HasValue)
        {
            LastSeen = lastSeen.Value;
        }
        OnPropertyChanged(nameof(OnlineStatusText));
    }

    public void UpdateProfile(string? fullName, string? avatarUrl, string? status)
    {
        if (!string.IsNullOrEmpty(fullName))
            FullName = fullName;
        
        if (avatarUrl != null)
            AvatarUrl = avatarUrl;
        
        if (!string.IsNullOrEmpty(status))
            Status = status;
        
        OnPropertyChanged(nameof(DisplayName));
        OnPropertyChanged(nameof(InitialsText));
    }

    partial void OnFullNameChanged(string value)
    {
        OnPropertyChanged(nameof(DisplayName));
        OnPropertyChanged(nameof(InitialsText));
    }

    partial void OnUserNameChanged(string value)
    {
        OnPropertyChanged(nameof(DisplayName));
        OnPropertyChanged(nameof(InitialsText));
    }

    partial void OnIsOnlineChanged(bool value)
    {
        OnPropertyChanged(nameof(OnlineStatusText));
    }

    partial void OnLastSeenChanged(DateTime value)
    {
        OnPropertyChanged(nameof(OnlineStatusText));
    }
}

