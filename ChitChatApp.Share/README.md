# ChitChatApp.Share Documentation

## Overview

**ChitChatApp.Share** is the shared library project that contains all the ViewModels, services interfaces, state management, validation logic, and data mapping profiles for the ChitChat application. This project follows the MVVM (Model-View-ViewModel) pattern and implements clean architecture principles by separating business logic from UI-specific code.

## Project Architecture

The project is structured to support multiple UI frameworks (AvaloniaUI, Blazor, etc.) by providing a shared foundation of ViewModels and services that can be consumed by any presentation layer.

### Target Framework
- **.NET 9.0** with nullable reference types enabled

### Key Dependencies
- **CommunityToolkit.Mvvm (v8.2.2)** - Provides MVVM base classes, observable properties, and relay commands
- **AutoMapper (v12.0.1)** - For object-to-object mapping between DTOs and ViewModels
- **FluentValidation (v12.0.0)** - For robust input validation with clean separation of concerns
- **ChitChatApp.Core** - Core domain logic and data access layer

## Folder Structure

```
ChitChatApp.Share/
├── Mapping/                    # AutoMapper profiles for DTO-ViewModel conversion
├── Services/                   # Service interfaces for cross-platform implementations
├── StateManagement/            # Global application state management
├── Validators/                 # FluentValidation validators for ViewModels
└── ViewModels/                 # MVVM ViewModels organized by feature
    ├── Authentication/         # Login and authentication related ViewModels
    ├── Base/                   # Base classes and interfaces
    ├── Chat/                   # Chat and messaging ViewModels
    ├── Items/                  # List item ViewModels for collections
    └── Main/                   # Main application and list ViewModels
```

---

## 📁 Mapping

### **DtoToViewModelProfile.cs**
**Purpose**: AutoMapper configuration profile for mapping Data Transfer Objects (DTOs) from ChitChatApp.Core to ViewModels.

**Key Features**:
- Provides placeholder mappings ready for when DTOs are implemented
- Will handle conversions between domain models and presentation models
- Includes example mappings for common scenarios (User → Contact, ChatRoom → ChatRoomItem, etc.)

**Usage**: Automatically used by AutoMapper when configured in DI container.

---

## 📁 Services

Service interfaces that define contracts for platform-specific implementations.

### **IDialogService.cs**
**Purpose**: Abstraction for showing various types of dialogs and user interactions.

**Key Methods**:
- `ShowConfirmationAsync()` - Yes/No confirmation dialogs
- `ShowAlertAsync()` - Information alerts with OK button
- `ShowErrorAsync()` - Error message dialogs
- `ShowPromptAsync()` - Text input dialogs
- `ShowOptionsAsync()` - Multiple choice selection
- `ShowFilePickerAsync()` - File selection dialogs
- `ShowFolderPickerAsync()` - Folder selection dialogs
- `ShowSaveFileDialogAsync()` - Save file dialogs
- `ShowLoading()` - Loading dialogs with progress support

**Implementation**: Each UI project (AvaloniaUI, Blazor) provides platform-specific implementations.

### **INotificationService.cs**
**Purpose**: Abstraction for showing toast notifications and alerts.

**Key Methods**:
- `ShowSuccessAsync()` - Success notifications
- `ShowErrorAsync()` - Error notifications  
- `ShowInfoAsync()` - Information notifications
- `ShowWarningAsync()` - Warning notifications
- `ShowCustomAsync()` - Custom notifications with icon types
- `ClearAllAsync()` - Clear all active notifications

**Features**:
- Configurable duration and auto-dismiss behavior
- Support for different notification types (Success, Error, Info, Warning, Question)
- Customizable titles and button text

---

## 📁 StateManagement

### **AppState.cs**
**Purpose**: Global application state management using observable properties.

**User Authentication State**:
- `IsAuthenticated` - Whether user is logged in
- `CurrentUserId` - Unique identifier of current user
- `CurrentUserName` - Username of current user
- `CurrentUserFullName` - Full display name
- `CurrentUserEmail` - Email address
- `CurrentUserAvatar` - Profile picture URL
- `DisplayName` - Computed property combining full name or username

**User Status**:
- `IsUserOnline` - Online/offline status
- `CurrentUserStatus` - Custom status message

**Chat State**:
- `ActiveeChatRoomId` - Currently open chat room
- `ActiveChatRoomName` - Name of active chat room
- `UnreadMessagesCount` - Total unread messages
- `UnreadChatsCount` - Number of chats with unread messages

**Connection State**:
- `IsConnectedToRealtime` - Real-time connection status
- `LastSyncTime` - Last successful sync timestamp

**Settings**:
- `IsDarkTheme` - Theme preference
- `CurrentLanguage` - Language setting (default: "en")
- `NotificationsEnabled` - Push notification setting
- `SoundEnabled` - Sound notification setting

**Key Methods**:
- `Login()` - Set user authentication state
- `Logout()` - Clear all user data and reset state
- `UpdateUserProfile()` - Update user profile information
- `SetActiveChat()` - Set currently active chat room
- `UpdateConnectionState()` - Update real-time connection status
- `UpdateUnreadCounts()` - Update notification badges
- `UpdateSettings()` - Change application settings

**Events**: Fires events when state changes occur for reactive programming.

---

## 📁 Validators

FluentValidation implementation for robust input validation.

### **IValidatableViewModel.cs**
**Purpose**: Interface that defines validation capabilities for ViewModels.

**Properties**:
- `ValidationResult` - Complete FluentValidation result
- `IsValid` - Boolean validation state
- `ValidationError` - Formatted error messages

**Methods**:
- `ValidateAsync()` - Perform validation asynchronously
- `ClearValidation()` - Reset validation state

### **LoginViewModelValidator.cs**
**Purpose**: Validates login form inputs.

**Validation Rules**:
- **Email**: Required, valid email format, maximum 254 characters
- **Password**: Required, minimum 6 characters, maximum 100 characters

### **MessageInputViewModelValidator.cs**
**Purpose**: Validates message input before sending.

**Validation Rules**:
- **Text**: Required when no attachment/recording, max 2000 characters, not just whitespace
- **AttachmentFileName**: Required when attachment present, max 255 characters
- **Content**: At least one input method must be used (text, attachment, or recording)

### **ContactListViewModelValidator.cs**
**Purpose**: Validates contact search and list operations.

**Validation Rules**:
- **SearchText**: Maximum 100 characters, alphanumeric plus @._- characters only

### **ValidationExample.cs**
**Purpose**: Demonstrates how to use validators with example test cases.

---

## 📁 ViewModels

### Authentication

#### **LoginViewModel.cs**
**Purpose**: Handles user authentication and login process.

**Properties**:
- `Email` - User email input
- `Password` - User password input  
- `RememberMe` - Remember login preference
- `IsLoginSuccessful` - Login result status

**Commands**:
- `LoginCommand` - Validates inputs and initiates login
- `NavigateToRegisterCommand` - Navigate to registration
- `ForgotPasswordCommand` - Initiate password recovery

**Events**:
- `LoginRequested` - Fired when login should be processed
- `RegisterRequested` - Navigate to registration
- `ForgotPasswordRequested` - Navigate to password reset

**Features**:
- Integrated FluentValidation
- Automatic input validation before processing
- Clear validation error display

### Base

#### **BaseViewModel.cs**
**Purpose**: Abstract base class providing common functionality for all ViewModels.

**Properties**:
- `IsBusy` - Loading state indicator
- `Title` - Page/view title
- `ErrorMessage` - Error display message
- `ValidationResult` - FluentValidation result
- `IsValid` - Validation state
- `ValidationError` - Validation error text

**Methods**:
- `ExecuteAsync()` - Safe async operation execution with error handling
- `ValidateAsync()` - Perform FluentValidation
- `ClearValidation()` - Reset validation state

**Features**:
- Implements `IValidatableViewModel` for validation support
- Implements `IDisposable` for proper resource cleanup
- Automatic error handling and loading state management
- Integration with FluentValidation framework

### Chat

#### **ChatViewModel.cs**
**Purpose**: Manages individual chat room conversations and real-time messaging.

**Properties**:
- `ChatRoomId` - Unique chat room identifier
- `ChatRoomName` - Display name of chat room
- `IsGroup` - Whether this is a group chat
- `ParticipantCount` - Number of participants
- `TypingStatus` - Who is currently typing
- `IsConnected` - Real-time connection status
- `IsLoadingMessages` - Loading state for message history
- `HasMoreMessages` - Whether more messages can be loaded

**Collections**:
- `Messages` - ObservableCollection of MessageViewModel objects

**Child ViewModels**:
- `MessageInput` - MessageInputViewModel for composing messages

**Commands**:
- `LoadMessagesCommand` - Load initial messages
- `LoadMoreMessagesCommand` - Load older messages (pagination)
- `MarkMessagesAsReadCommand` - Mark messages as read
- `ShowChatRoomInfoCommand` - Show chat room details

**Events**:
- `MessageSent` - When a new message is sent
- `MessageReceived` - When a new message is received
- `TypingStarted/Stopped` - Typing indicator events
- `ChatRoomInfoRequested` - Show chat room info
- `AttachmentRequested` - File attachment request

**Key Methods**:
- `InitializeChat()` - Set up chat room
- `AddMessage()` - Add new message to conversation
- `UpdateTypingStatus()` - Update typing indicators
- `UpdateConnectionStatus()` - Update real-time connection
- `UpdateParticipantCount()` - Update member count

#### **MessageInputViewModel.cs**
**Purpose**: Handles message composition with text, attachments, and voice recording.

**Properties**:
- `Text` - Message text content
- `CanSend` - Whether message can be sent
- `IsRecording` - Voice recording state
- `ReplyingTo` - Message being replied to
- `EditingMessage` - Message being edited
- `HasAttachment` - Whether attachment is present
- `AttachmentPreview` - Attachment preview URL
- `AttachmentFileName` - Name of attached file

**Commands**:
- `SendMessageCommand` - Send the composed message
- `AddAttachmentCommand` - Add file attachment
- `StartVoiceRecordingCommand` - Start voice recording
- `StopVoiceRecordingCommand` - Stop voice recording
- `OpenEmojiPickerCommand` - Open emoji selector
- `CancelReplyCommand` - Cancel reply action
- `CancelEditCommand` - Cancel edit action
- `RemoveAttachmentCommand` - Remove attachment

**Events**:
- `MessageSent` - When message is sent
- `TypingStarted/Stopped` - Typing indicator events
- `AttachmentRequested` - File picker request
- `VoiceRecordingRequested` - Voice recording request
- `EmojiPickerRequested` - Emoji picker request

**Features**:
- Automatic typing indicator management with timer
- Support for reply and edit modes
- FluentValidation integration
- Multi-input support (text, attachment, voice)

#### **MessageViewModel.cs**
**Purpose**: Represents individual messages in chat conversations.

**Properties**:
- `MessageId` - Unique message identifier
- `SenderId` - User ID of sender
- `SenderName` - Display name of sender
- `SenderAvatar` - Sender's profile picture
- `Content` - Message text content
- `MessageType` - Type: text, image, file, system
- `SentAt` - Timestamp when sent
- `IsFromCurrentUser` - Whether sent by current user
- `IsRead` - Whether message has been read
- `IsDelivered` - Delivery status
- `IsEdited` - Whether message was edited
- `EditedAt` - Edit timestamp
- `AttachmentUrl` - URL of attached file
- `AttachmentFileName` - Name of attached file
- `AttachmentFileSize` - Size of attached file
- `IsSelected` - Selection state for actions
- `CanEdit` - Whether user can edit this message
- `CanDelete` - Whether user can delete this message

**Computed Properties**:
- `FormattedTime` - Display time (HH:mm)
- `FormattedDate` - Display date (dd/MM/yyyy)
- `FormattedDateTime` - Full timestamp
- `IsToday/IsYesterday` - Date comparison helpers
- `RelativeTime` - Human-readable time ("2m ago", "1h ago")
- `HasAttachment` - Whether message has attachment
- `AttachmentSizeFormatted` - Human-readable file size

**Commands**:
- `ReplyCommand` - Reply to this message
- `EditCommand` - Edit message content
- `DeleteCommand` - Delete this message
- `ForwardCommand` - Forward message to other chats
- `DownloadAttachmentCommand` - Download attached file
- `CopyTextCommand` - Copy message text to clipboard

**Events**:
- `MessageReply` - Reply action requested
- `MessageEdit` - Edit action requested
- `MessageDelete` - Delete action requested
- `MessageForward` - Forward action requested
- `AttachmentDownload` - Download attachment requested

**Key Methods**:
- `MarkAsRead()` - Mark message as read
- `MarkAsDelivered()` - Mark as delivered
- `UpdateEditedContent()` - Update content after edit

**Features**:
- Automatic permission calculation based on time and ownership
- Smart time formatting and relative display
- File size formatting with appropriate units
- Support for various message types and attachments

### Items

#### **ChatRoomListItemViewModel.cs**
**Purpose**: Represents individual chat rooms in the chat room list.

**Properties**:
- `ChatRoomId` - Unique identifier
- `Name` - Chat room display name
- `IsGroup` - Group chat flag
- `AvatarUrl` - Room avatar image
- `LastMessage` - Preview of last message
- `LastMessageSender` - Who sent last message
- `LastMessageTime` - When last message was sent
- `UnreadCount` - Number of unread messages
- `IsPinned` - Whether room is pinned to top
- `IsMuted` - Whether notifications are muted
- `IsArchived` - Whether room is archived
- `IsSelected` - Selection state
- `ParticipantCount` - Number of members
- `IsTyping` - Whether someone is typing
- `TypingUserName` - Who is typing
- `CreatedAt` - Room creation timestamp

**Computed Properties**:
- `DisplayName` - Formatted display name
- `LastMessageDisplay` - Formatted last message with typing indicator
- `LastMessageTimeDisplay` - Human-readable timestamp
- `HasUnreadMessages` - Whether there are unread messages
- `UnreadCountDisplay` - Formatted unread count ("99+" for >99)
- `InitialsText` - Initials for avatar fallback
- `ParticipantCountDisplay` - Formatted member count for groups

**Commands**:
- `SelectCommand` - Select this chat room
- `TogglePinCommand` - Pin/unpin room
- `ToggleMuteCommand` - Mute/unmute notifications
- `ToggleArchiveCommand` - Archive/unarchive room
- `LeaveCommand` - Leave the chat room
- `DeleteCommand` - Delete the chat room

**Events**:
- `ChatRoomSelected` - Room selection event
- `PinToggled` - Pin state changed
- `MuteToggled` - Mute state changed
- `ArchiveToggled` - Archive state changed
- `LeaveRequested` - Leave action requested
- `DeleteRequested` - Delete action requested

**Key Methods**:
- `UpdateLastMessage()` - Update last message info and increment unread count
- `MarkAsRead()` - Clear unread count
- `UpdateTypingStatus()` - Update typing indicator
- `UpdateParticipantCount()` - Update member count

#### **ContactListItemViewModel.cs**
**Purpose**: Represents individual contacts in the contact list.

**Properties**:
- `UserId` - Unique user identifier
- `UserName` - Username
- `FullName` - Full display name
- `Email` - Email address
- `AvatarUrl` - Profile picture URL
- `IsOnline` - Online status
- `LastSeen` - Last seen timestamp
- `Status` - Custom status message
- `IsBlocked` - Whether contact is blocked
- `IsFavorite` - Whether contact is favorited
- `IsSelected` - Selection state
- `AddedAt` - When contact was added

**Computed Properties**:
- `DisplayName` - Formatted display name (full name or username)
- `OnlineStatusText` - Human-readable online status with last seen
- `InitialsText` - Initials for avatar fallback

**Commands**:
- `SelectCommand` - Select this contact
- `StartChatCommand` - Start conversation with contact
- `BlockCommand` - Block this contact
- `UnblockCommand` - Unblock this contact
- `RemoveCommand` - Remove from contacts
- `ToggleFavoriteCommand` - Add/remove from favorites

**Events**:
- `ContactSelected` - Contact selection event
- `StartChatRequested` - Start chat action
- `BlockRequested` - Block action requested
- `UnblockRequested` - Unblock action requested
- `RemoveRequested` - Remove action requested
- `FavoriteToggled` - Favorite state changed

**Key Methods**:
- `UpdateOnlineStatus()` - Update online/offline status
- `UpdateProfile()` - Update profile information

### Main

#### **ChatRoomListViewModel.cs**
**Purpose**: Manages the list of chat rooms with search and filtering capabilities.

**Properties**:
- `SelectedChatRoom` - Currently selected chat room
- `SearchText` - Search filter text
- `IsRefreshing` - Refresh operation state

**Collections**:
- `ChatRooms` - All chat rooms
- `FilteredChatRooms` - Filtered results based on search

**Commands**:
- `SelectChatRoomCommand` - Select a chat room
- `RefreshCommand` - Refresh chat room list
- `CreateGroupCommand` - Create new group chat
- `LeaveChatRoomCommand` - Leave selected chat room
- `DeleteChatRoomCommand` - Delete selected chat room

**Events**:
- `ChatRoomSelected` - Chat room selection event
- `ChatRoomLeft` - Leave chat room event
- `ChatRoomDeleted` - Delete chat room event
- `CreateGroupRequested` - Create group request

**Key Methods**:
- `LoadChatRoomsAsync()` - Load chat rooms from service
- `AddChatRoom()` - Add new chat room to list
- `UpdateLastMessage()` - Update last message for specific room
- `MarkAsRead()` - Clear unread count for specific room
- `UpdateTypingStatus()` - Update typing indicator for room

**Features**:
- Real-time search filtering by name and last message
- Automatic sorting by last message time
- Pull-to-refresh support

#### **ContactListViewModel.cs**
**Purpose**: Manages the contact list with search and contact management.

**Properties**:
- `SelectedContact` - Currently selected contact
- `SearchText` - Search filter text
- `IsRefreshing` - Refresh operation state

**Collections**:
- `Contacts` - All contacts
- `FilteredContacts` - Filtered results based on search

**Commands**:
- `SelectContactCommand` - Select a contact
- `RefreshCommand` - Refresh contact list
- `AddContactCommand` - Add new contact
- `BlockContactCommand` - Block selected contact
- `RemoveContactCommand` - Remove selected contact

**Events**:
- `ContactSelected` - Contact selection event
- `ContactBlocked` - Contact blocked event
- `ContactRemoved` - Contact removed event
- `AddContactRequested` - Add contact request

**Key Methods**:
- `LoadContactsAsync()` - Load contacts from service
- `AddContact()` - Add new contact to list
- `UpdateContactStatus()` - Update online status for specific contact

**Features**:
- Real-time search filtering by name and username
- Contact management operations (block, remove, add)

#### **MainViewModel.cs**
**Purpose**: Root ViewModel for the main application interface, coordinating between contacts and chat rooms.

**Properties**:
- `SelectedContact` - Currently selected contact
- `SelectedChatRoom` - Currently selected chat room
- `CurrentUserName` - Current user's display name
- `CurrentUserAvatar` - Current user's profile picture
- `IsUserOnline` - Current user's online status
- `SearchText` - Global search text

**Collections**:
- `Contacts` - Contact list
- `ChatRooms` - Chat room list

**Commands**:
- `SelectContactCommand` - Select contact (deselects chat room)
- `SelectChatRoomCommand` - Select chat room (deselects contact)
- `SearchCommand` - Global search functionality
- `AddContactCommand` - Add new contact
- `CreateGroupCommand` - Create new group
- `ToggleOnlineStatusCommand` - Toggle user online status
- `OpenSettingsCommand` - Open settings
- `LogoutCommand` - Logout user

**Events**:
- `ContactSelected` - Contact selection event
- `ChatRoomSelected` - Chat room selection event
- `LogoutRequested` - Logout request
- `SettingsRequested` - Settings request
- `AddContactRequested` - Add contact request
- `CreateGroupRequested` - Create group request

**Key Methods**:
- `LoadDataAsync()` - Load all application data
- `UpdateUserInfo()` - Update current user information

**Features**:
- Mutual exclusive selection (contact OR chat room)
- Global search across contacts and chat rooms
- Central coordination point for main UI

---

## Design Patterns Used

### MVVM (Model-View-ViewModel)
- **ViewModels** handle business logic and state management
- **Models** are defined in ChitChatApp.Core
- **Views** are implemented in UI-specific projects

### Observer Pattern
- Extensive use of `ObservableCollection<T>` for automatic UI updates
- `INotifyPropertyChanged` implementation via CommunityToolkit.Mvvm
- Event-driven communication between ViewModels

### Command Pattern
- All user actions implemented as `IRelayCommand` using CommunityToolkit.Mvvm
- Consistent command interface across all ViewModels

### Dependency Inversion
- Service interfaces defined in shared library
- Platform-specific implementations injected at runtime

### Validation Pattern
- FluentValidation for robust input validation
- Separation of validation logic from ViewModels
- Reusable validation rules

## Key Features

### ✅ **Real-time Updates**
- ObservableCollection usage ensures automatic UI updates
- Event-driven architecture for real-time notifications
- Typing indicators and presence updates

### ✅ **Robust Validation**
- FluentValidation integration with comprehensive rules
- User-friendly error messages
- Validation state binding for UI feedback

### ✅ **Cross-Platform Ready**
- Shared ViewModels work across AvaloniaUI, Blazor, and other frameworks
- Platform-specific services injected via interfaces
- No UI framework dependencies

### ✅ **Performance Optimized**
- Lazy loading with pagination support
- Efficient search and filtering
- Proper resource disposal and memory management

### ✅ **User Experience**
- Comprehensive error handling and loading states
- Intuitive command availability (CanExecute logic)
- Rich formatting and display helpers

### ✅ **Maintainable Code**
- Clean separation of concerns
- Comprehensive documentation and examples
- Consistent patterns throughout

## Usage Examples

### Basic ViewModel Usage
```csharp
// Create and use a ViewModel
var loginViewModel = new LoginViewModel();
loginViewModel.Email = "user@example.com";
loginViewModel.Password = "password123";

// Validate before processing
if (await loginViewModel.ValidateAsync())
{
    // Process login
}
else
{
    // Show validation errors
    Console.WriteLine(loginViewModel.ValidationError);
}
```

### Handling Events
```csharp
var chatViewModel = new ChatViewModel();
chatViewModel.MessageSent += (sender, message) => {
    // Handle message sent
};

chatViewModel.TypingStarted += (sender, text) => {
    // Show typing indicator
};
```

### Service Integration
```csharp
// In UI project, inject platform-specific implementations
services.AddSingleton<IDialogService, PlatformDialogService>();
services.AddSingleton<INotificationService, PlatformNotificationService>();
```

## Testing

The validation system includes test examples in `ValidationExample.cs` demonstrating:
- Input validation scenarios
- Error handling
- Success cases

ViewModels are designed to be easily unit testable with mocked services.

## Future Enhancements

- **Localization**: Multi-language support for validation messages
- **Offline Support**: Local caching and sync capabilities  
- **Advanced Search**: Full-text search with highlighting
- **Message Threading**: Reply chains and conversation threading
- **Custom Themes**: User-configurable appearance settings
- **Voice Messages**: Audio recording and playback support
- **File Sharing**: Advanced attachment handling with previews

---

This documentation provides a complete overview of the ChitChatApp.Share project structure and functionality. Each class is designed with specific responsibilities while working together to create a cohesive, maintainable, and feature-rich chat application foundation.

