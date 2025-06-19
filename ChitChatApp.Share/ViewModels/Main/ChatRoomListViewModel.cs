using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ChitChatApp.Share.ViewModels.Base;
using ChitChatApp.Share.ViewModels.Items;

namespace ChitChatApp.Share.ViewModels.Main;

public partial class ChatRoomListViewModel : BaseViewModel
{
    [ObservableProperty]
    private ChatRoomListItemViewModel? selectedChatRoom;

    [ObservableProperty]
    private string searchText = string.Empty;

    [ObservableProperty]
    private bool isRefreshing;

    public ObservableCollection<ChatRoomListItemViewModel> ChatRooms { get; } = new();
    public ObservableCollection<ChatRoomListItemViewModel> FilteredChatRooms { get; } = new();

    public event EventHandler<ChatRoomListItemViewModel>? ChatRoomSelected;
    public event EventHandler<ChatRoomListItemViewModel>? ChatRoomLeft;
    public event EventHandler<ChatRoomListItemViewModel>? ChatRoomDeleted;
    public event EventHandler? CreateGroupRequested;

    public ChatRoomListViewModel()
    {
        Title = "Chat Rooms";
    }

    [RelayCommand]
    private void SelectChatRoom(ChatRoomListItemViewModel chatRoom)
    {
        SelectedChatRoom = chatRoom;
        ChatRoomSelected?.Invoke(this, chatRoom);
    }

    [RelayCommand]
    private async Task RefreshAsync()
    {
        await ExecuteAsync(async () =>
        {
            IsRefreshing = true;
            await LoadChatRoomsAsync();
            IsRefreshing = false;
        }, showBusy: false);
    }

    [RelayCommand]
    private void CreateGroup()
    {
        CreateGroupRequested?.Invoke(this, EventArgs.Empty);
    }

    [RelayCommand]
    private async Task LeaveChatRoomAsync(ChatRoomListItemViewModel chatRoom)
    {
        await ExecuteAsync(async () =>
        {
            // This would typically call a service to leave the chat room
            await Task.Delay(500);
            
            ChatRoomLeft?.Invoke(this, chatRoom);
            ChatRooms.Remove(chatRoom);
            FilterChatRooms();
        });
    }

    [RelayCommand]
    private async Task DeleteChatRoomAsync(ChatRoomListItemViewModel chatRoom)
    {
        await ExecuteAsync(async () =>
        {
            // This would typically call a service to delete the chat room
            await Task.Delay(500);
            
            ChatRoomDeleted?.Invoke(this, chatRoom);
            ChatRooms.Remove(chatRoom);
            FilterChatRooms();
        });
    }

    public async Task LoadChatRoomsAsync()
    {
        await ExecuteAsync(async () =>
        {
            // This would typically load chat rooms from a service
            await Task.Delay(1000);
            
            // Clear existing chat rooms
            ChatRooms.Clear();
            
            // Add sample chat rooms (replace with actual service call)
            // var chatRooms = await chatRoomService.GetChatRoomsForUserAsync();
            // foreach (var chatRoom in chatRooms)
            // {
            //     ChatRooms.Add(new ChatRoomListItemViewModel(chatRoom));
            // }
            
            FilterChatRooms();
        });
    }

    private void FilterChatRooms()
    {
        FilteredChatRooms.Clear();
        
        var filtered = string.IsNullOrWhiteSpace(SearchText)
            ? ChatRooms
            : ChatRooms.Where(cr => cr.Name.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                                   (cr.LastMessage?.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ?? false));

        foreach (var chatRoom in filtered.OrderByDescending(cr => cr.LastMessageTime))
        {
            FilteredChatRooms.Add(chatRoom);
        }
    }

    partial void OnSearchTextChanged(string value)
    {
        FilterChatRooms();
    }

    public void AddChatRoom(ChatRoomListItemViewModel chatRoom)
    {
        ChatRooms.Add(chatRoom);
        FilterChatRooms();
    }

    public void UpdateLastMessage(Guid chatRoomId, string message, DateTime timestamp, string senderName)
    {
        var chatRoom = ChatRooms.FirstOrDefault(cr => cr.ChatRoomId == chatRoomId);
        if (chatRoom != null)
        {
            chatRoom.LastMessage = message;
            chatRoom.LastMessageTime = timestamp;
            chatRoom.LastMessageSender = senderName;
            chatRoom.UnreadCount++;
            
            // Re-filter to update sorting
            FilterChatRooms();
        }
    }

    public void MarkAsRead(Guid chatRoomId)
    {
        var chatRoom = ChatRooms.FirstOrDefault(cr => cr.ChatRoomId == chatRoomId);
        if (chatRoom != null)
        {
            chatRoom.UnreadCount = 0;
        }
    }

    public void UpdateTypingStatus(Guid chatRoomId, bool isTyping, string? typingUserName = null)
    {
        var chatRoom = ChatRooms.FirstOrDefault(cr => cr.ChatRoomId == chatRoomId);
        if (chatRoom != null)
        {
            chatRoom.IsTyping = isTyping;
            chatRoom.TypingUserName = typingUserName;
        }
    }
}

