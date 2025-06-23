using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ChitChatApp.Share.ViewModels.Base;
using ChitChatApp.Share.ViewModels.Items;

namespace ChitChatApp.Share.ViewModels.Main;

public partial class MainViewModel : BaseViewModel
{
    [ObservableProperty]
    private ContactListItemViewModel? selectedContact;

    [ObservableProperty]
    private ChatRoomListItemViewModel? selectedChatRoom;

    [ObservableProperty]
    private string? currentUserName;

    [ObservableProperty]
    private string? currentUserAvatar;

    [ObservableProperty]
    private bool isUserOnline = true;

    [ObservableProperty]
    private string searchText = string.Empty;

    public ObservableCollection<ContactListItemViewModel> Contacts { get; } = new();
    public ObservableCollection<ChatRoomListItemViewModel> ChatRooms { get; } = new();

    public event EventHandler<ContactListItemViewModel>? ContactSelected;
    public event EventHandler<ChatRoomListItemViewModel>? ChatRoomSelected;
    public event EventHandler? LogoutRequested;
    public event EventHandler? SettingsRequested;
    public event EventHandler? AddContactRequested;
    public event EventHandler? CreateGroupRequested;

    public MainViewModel()
    {
        Title = "ChitChat";
    }

    [RelayCommand]
    private void SelectContact(ContactListItemViewModel contact)
    {
        SelectedContact = contact;
        SelectedChatRoom = null;
        ContactSelected?.Invoke(this, contact);
    }

    [RelayCommand]
    private void SelectChatRoom(ChatRoomListItemViewModel chatRoom)
    {
        SelectedChatRoom = chatRoom;
        SelectedContact = null;
        ChatRoomSelected?.Invoke(this, chatRoom);
    }

    [RelayCommand]
    private async Task SearchAsync()
    {
        await ExecuteAsync(async () =>
        {
            // Implement search functionality
            await Task.Delay(500); // Simulate search delay
            
            // Filter contacts and chat rooms based on SearchText
            // This would typically call a search service
        });
    }

    [RelayCommand]
    private void AddContact()
    {
        AddContactRequested?.Invoke(this, EventArgs.Empty);
    }

    [RelayCommand]
    private void CreateGroup()
    {
        CreateGroupRequested?.Invoke(this, EventArgs.Empty);
    }

    [RelayCommand]
    private void ToggleOnlineStatus()
    {
        IsUserOnline = !IsUserOnline;
        // This would typically call a service to update user status
    }

    [RelayCommand]
    private void OpenSettings()
    {
        SettingsRequested?.Invoke(this, EventArgs.Empty);
    }

    [RelayCommand]
    private void Logout()
    {
        LogoutRequested?.Invoke(this, EventArgs.Empty);
    }

    public async Task LoadDataAsync()
    {
        await ExecuteAsync(async () =>
        {
            await LoadContactsAsync();
            await LoadChatRoomsAsync();
        });
    }

    private async Task LoadContactsAsync()
    {
        // This would typically load contacts from a service
        await Task.Delay(100); // Simulate network delay
        
        // Add sample data for now
        Contacts.Clear();
        // Contacts would be populated from actual data
    }

    private async Task LoadChatRoomsAsync()
    {
        // This would typically load chat rooms from a service
        await Task.Delay(100); // Simulate network delay
        
        // Add sample data for now
        ChatRooms.Clear();
        // Chat rooms would be populated from actual data
    }

    public void UpdateUserInfo(string userName, string? avatarUrl)
    {
        CurrentUserName = userName;
        CurrentUserAvatar = avatarUrl;
    }

    partial void OnSearchTextChanged(string value)
    {
        // Trigger search when text changes
        SearchCommand.Execute(null);
    }
}

