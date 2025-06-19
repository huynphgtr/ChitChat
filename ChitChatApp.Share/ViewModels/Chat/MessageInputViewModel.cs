using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ChitChatApp.Share.ViewModels.Base;
using ChitChatApp.Share.Validators;
using System.Timers;

namespace ChitChatApp.Share.ViewModels.Chat;

public partial class MessageInputViewModel : BaseViewModel
{
    private readonly System.Timers.Timer _typingTimer;
    private bool _isTyping;

    [ObservableProperty]
    private string text = string.Empty;

    [ObservableProperty]
    private bool canSend;

    [ObservableProperty]
    private bool isRecording;

    [ObservableProperty]
    private MessageViewModel? replyingTo;

    [ObservableProperty]
    private MessageViewModel? editingMessage;

    [ObservableProperty]
    private bool hasAttachment;

    [ObservableProperty]
    private string? attachmentPreview;

    [ObservableProperty]
    private string? attachmentFileName;

    public event EventHandler<string>? MessageSent;
    public event EventHandler? TypingStarted;
    public event EventHandler? TypingStopped;
    public event EventHandler? AttachmentRequested;
    public event EventHandler? VoiceRecordingRequested;
    public event EventHandler? EmojiPickerRequested;

    public MessageInputViewModel()
    {
        _typingTimer = new System.Timers.Timer(2000); // 2 seconds
        _typingTimer.Elapsed += OnTypingTimerElapsed;
        _typingTimer.AutoReset = false;
        Validator = new MessageInputViewModelValidator();
    }

    [RelayCommand]
    private async Task SendMessageAsync()
    {
        // Validate using FluentValidation
        if (!await ValidateAsync())
        {
            // Validation errors are automatically displayed via ValidationError property
            return;
        }

        var messageText = Text.Trim();
        
        // Stop typing indicator
        StopTyping();
        
        // Clear input
        Text = string.Empty;
        ClearReply();
        ClearEdit();
        ClearAttachment();
        ClearValidation();

        // Send the message
        MessageSent?.Invoke(this, messageText);
    }

    [RelayCommand]
    private void AddAttachment()
    {
        AttachmentRequested?.Invoke(this, EventArgs.Empty);
    }

    [RelayCommand]
    private void StartVoiceRecording()
    {
        if (!IsRecording)
        {
            IsRecording = true;
            VoiceRecordingRequested?.Invoke(this, EventArgs.Empty);
        }
    }

    [RelayCommand]
    private void StopVoiceRecording()
    {
        if (IsRecording)
        {
            IsRecording = false;
            // The actual voice message would be processed and sent here
        }
    }

    [RelayCommand]
    private void OpenEmojiPicker()
    {
        EmojiPickerRequested?.Invoke(this, EventArgs.Empty);
    }

    [RelayCommand]
    private void CancelReply()
    {
        ClearReply();
    }

    [RelayCommand]
    private void CancelEdit()
    {
        ClearEdit();
    }

    [RelayCommand]
    private void RemoveAttachment()
    {
        ClearAttachment();
    }

    public void SetReplyTo(MessageViewModel message)
    {
        ReplyingTo = message;
        EditingMessage = null;
    }

    public void SetEditMessage(MessageViewModel message)
    {
        EditingMessage = message;
        ReplyingTo = null;
        Text = message.Content;
    }

    public void SetAttachment(string fileName, string? previewUrl = null)
    {
        AttachmentFileName = fileName;
        AttachmentPreview = previewUrl;
        HasAttachment = true;
    }

    public void AddEmoji(string emoji)
    {
        Text += emoji;
    }

    public void Reset()
    {
        Text = string.Empty;
        ClearReply();
        ClearEdit();
        ClearAttachment();
        StopTyping();
    }

    private void ClearReply()
    {
        ReplyingTo = null;
    }

    private void ClearEdit()
    {
        EditingMessage = null;
    }

    private void ClearAttachment()
    {
        HasAttachment = false;
        AttachmentFileName = null;
        AttachmentPreview = null;
    }

    private void StartTyping()
    {
        if (!_isTyping)
        {
            _isTyping = true;
            TypingStarted?.Invoke(this, EventArgs.Empty);
        }
        
        // Reset the timer
        _typingTimer.Stop();
        _typingTimer.Start();
    }

    private void StopTyping()
    {
        if (_isTyping)
        {
            _isTyping = false;
            _typingTimer.Stop();
            TypingStopped?.Invoke(this, EventArgs.Empty);
        }
    }

    private void OnTypingTimerElapsed(object? sender, ElapsedEventArgs e)
    {
        StopTyping();
    }

    private void UpdateCanSend()
    {
        CanSend = !string.IsNullOrWhiteSpace(Text) || HasAttachment || IsRecording;
    }

    partial void OnTextChanged(string value)
    {
        UpdateCanSend();
        
        if (!string.IsNullOrWhiteSpace(value))
        {
            StartTyping();
        }
        else
        {
            StopTyping();
        }
    }

    partial void OnHasAttachmentChanged(bool value)
    {
        UpdateCanSend();
    }

    partial void OnIsRecordingChanged(bool value)
    {
        UpdateCanSend();
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _typingTimer?.Dispose();
        }
        base.Dispose(disposing);
    }
}

