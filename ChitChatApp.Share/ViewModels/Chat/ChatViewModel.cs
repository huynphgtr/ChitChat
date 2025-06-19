using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ChitChatApp.Share.ViewModels.Base;

namespace ChitChatApp.Share.ViewModels.Chat;

public partial class ChatViewModel : BaseViewModel
{
    [ObservableProperty]
    private Guid chatRoomId;

    [ObservableProperty]
    private string chatRoomName = string.Empty;

    [ObservableProperty]
    private bool isGroup;

    [ObservableProperty]
    private int participantCount;

    [ObservableProperty]
    private string typingStatus = string.Empty;

    [ObservableProperty]
    private bool isConnected = true;

    [ObservableProperty]
    private bool isLoadingMessages;

    [ObservableProperty]
    private bool hasMoreMessages = true;

    public ObservableCollection<MessageViewModel> Messages { get; } = new();
    public MessageInputViewModel MessageInput { get; }

    public event EventHandler<MessageViewModel>? MessageSent;
    public event EventHandler<MessageViewModel>? MessageReceived;
    public event EventHandler<string>? TypingStarted;
    public event EventHandler? TypingStopped;
    public event EventHandler? ChatRoomInfoRequested;
    public event EventHandler? AttachmentRequested;

    public ChatViewModel()
    {
        MessageInput = new MessageInputViewModel();
        MessageInput.MessageSent += OnMessageInputSent;
        MessageInput.TypingStarted += OnTypingStarted;
        MessageInput.TypingStopped += OnTypingStopped;
        MessageInput.AttachmentRequested += OnAttachmentRequested;
    }

    [RelayCommand]
    private async Task LoadMessagesAsync()
    {
        await ExecuteAsync(async () =>
        {
            IsLoadingMessages = true;
            
            // This would typically load messages from a service
            await Task.Delay(1000);
            
            // var messages = await messageService.GetMessagesAsync(ChatRoomId, skip: Messages.Count);
            // foreach (var message in messages.Reverse())
            // {
            //     Messages.Insert(0, new MessageViewModel(message));
            // }
            
            IsLoadingMessages = false;
        }, showBusy: false);
    }

    [RelayCommand]
    private async Task LoadMoreMessagesAsync()
    {
        if (IsLoadingMessages || !HasMoreMessages)
            return;

        await ExecuteAsync(async () =>
        {
            IsLoadingMessages = true;
            
            // This would typically load older messages from a service
            await Task.Delay(500);
            
            // var olderMessages = await messageService.GetMessagesAsync(ChatRoomId, skip: Messages.Count);
            // if (olderMessages.Any())
            // {
            //     foreach (var message in olderMessages.Reverse())
            //     {
            //         Messages.Insert(0, new MessageViewModel(message));
            //     }
            // }
            // else
            // {
            //     HasMoreMessages = false;
            // }
            
            IsLoadingMessages = false;
        }, showBusy: false);
    }

    [RelayCommand]
    private void ShowChatRoomInfo()
    {
        ChatRoomInfoRequested?.Invoke(this, EventArgs.Empty);
    }

    [RelayCommand]
    private async Task MarkMessagesAsReadAsync()
    {
        await ExecuteAsync(async () =>
        {
            // This would typically mark messages as read in the service
            await Task.Delay(100);
            
            foreach (var message in Messages.Where(m => !m.IsRead && !m.IsFromCurrentUser))
            {
                message.IsRead = true;
            }
        }, showBusy: false);
    }

    public void InitializeChat(Guid chatRoomId, string chatRoomName, bool isGroup)
    {
        ChatRoomId = chatRoomId;
        ChatRoomName = chatRoomName;
        IsGroup = isGroup;
        Title = chatRoomName;
        
        Messages.Clear();
        MessageInput.Reset();
    }

    public void AddMessage(MessageViewModel message)
    {
        Messages.Add(message);
        MessageReceived?.Invoke(this, message);
    }

    public void UpdateTypingStatus(string typingUsers)
    {
        TypingStatus = typingUsers;
    }

    public void UpdateConnectionStatus(bool isConnected)
    {
        IsConnected = isConnected;
    }

    public void UpdateParticipantCount(int count)
    {
        ParticipantCount = count;
    }

    private void OnMessageInputSent(object? sender, string message)
    {
        var messageViewModel = new MessageViewModel
        {
            Content = message,
            SentAt = DateTime.Now,
            SenderName = "You", // This would be the current user's name
            IsFromCurrentUser = true,
            MessageType = "text"
        };

        Messages.Add(messageViewModel);
        MessageSent?.Invoke(this, messageViewModel);
    }

    private void OnTypingStarted(object? sender, EventArgs e)
    {
        TypingStarted?.Invoke(this, MessageInput.Text);
    }

    private void OnTypingStopped(object? sender, EventArgs e)
    {
        TypingStopped?.Invoke(this, EventArgs.Empty);
    }

    private void OnAttachmentRequested(object? sender, EventArgs e)
    {
        AttachmentRequested?.Invoke(this, EventArgs.Empty);
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            MessageInput.MessageSent -= OnMessageInputSent;
            MessageInput.TypingStarted -= OnTypingStarted;
            MessageInput.TypingStopped -= OnTypingStopped;
            MessageInput.AttachmentRequested -= OnAttachmentRequested;
        }
        base.Dispose(disposing);
    }
}

