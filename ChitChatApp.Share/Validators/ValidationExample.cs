using ChitChatApp.Share.ViewModels.Authentication;
using ChitChatApp.Share.ViewModels.Chat;

namespace ChitChatApp.Share.Validators;

/// <summary>
/// Example class demonstrating how to use the validators
/// This is for demonstration purposes and testing
/// </summary>
public static class ValidationExample
{
    /// <summary>
    /// Example of how to test LoginViewModel validation
    /// </summary>
    public static async Task<bool> TestLoginValidation()
    {
        var loginViewModel = new LoginViewModel();
        
        // Test with invalid data
        loginViewModel.Email = "invalid-email";
        loginViewModel.Password = "123"; // Too short
        
        bool isValid = await loginViewModel.ValidateAsync();
        if (isValid)
        {
            throw new InvalidOperationException("Validation should have failed");
        }
        
        // Test with valid data
        loginViewModel.Email = "user@example.com";
        loginViewModel.Password = "validpassword123";
        
        isValid = await loginViewModel.ValidateAsync();
        if (!isValid)
        {
            throw new InvalidOperationException($"Validation should have passed: {loginViewModel.ValidationError}");
        }
        
        return true;
    }
    
    /// <summary>
    /// Example of how to test MessageInputViewModel validation
    /// </summary>
    public static async Task<bool> TestMessageInputValidation()
    {
        var messageInputViewModel = new MessageInputViewModel();
        
        // Test with no content
        messageInputViewModel.Text = "";
        messageInputViewModel.HasAttachment = false;
        messageInputViewModel.IsRecording = false;
        
        bool isValid = await messageInputViewModel.ValidateAsync();
        if (isValid)
        {
            throw new InvalidOperationException("Validation should have failed for empty content");
        }
        
        // Test with valid text
        messageInputViewModel.Text = "Hello, this is a valid message!";
        
        isValid = await messageInputViewModel.ValidateAsync();
        if (!isValid)
        {
            throw new InvalidOperationException($"Validation should have passed: {messageInputViewModel.ValidationError}");
        }
        
        // Test with attachment but no filename
        messageInputViewModel.Text = "";
        messageInputViewModel.HasAttachment = true;
        messageInputViewModel.AttachmentFileName = "";
        
        isValid = await messageInputViewModel.ValidateAsync();
        if (isValid)
        {
            throw new InvalidOperationException("Validation should have failed for attachment without filename");
        }
        
        // Test with valid attachment
        messageInputViewModel.AttachmentFileName = "document.pdf";
        
        isValid = await messageInputViewModel.ValidateAsync();
        if (!isValid)
        {
            throw new InvalidOperationException($"Validation should have passed: {messageInputViewModel.ValidationError}");
        }
        
        return true;
    }
}

