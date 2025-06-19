using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ChitChatApp.Share.ViewModels.Base;
using ChitChatApp.Share.Validators;

namespace ChitChatApp.Share.ViewModels.Authentication;

public partial class LoginViewModel : BaseViewModel
{
    [ObservableProperty]
    private string email = string.Empty;

    [ObservableProperty]
    private string password = string.Empty;

    [ObservableProperty]
    private bool rememberMe;

    [ObservableProperty]
    private bool isLoginSuccessful;

    public event EventHandler? LoginRequested;
    public event EventHandler? RegisterRequested;
    public event EventHandler? ForgotPasswordRequested;

    public LoginViewModel()
    {
        Title = "Login";
        Validator = new LoginViewModelValidator();
    }

    [RelayCommand]
    private async Task LoginAsync()
    {
        // Validate using FluentValidation
        if (!await ValidateAsync())
        {
            ErrorMessage = ValidationError;
            return;
        }

        await ExecuteAsync(async () =>
        {
            // Clear any previous validation errors
            ClearValidation();
            
            // Simulate login operation
            await Task.Delay(1000);
            
            // This would typically call an authentication service
            LoginRequested?.Invoke(this, EventArgs.Empty);
            IsLoginSuccessful = true;
        });
    }

    [RelayCommand]
    private void NavigateToRegister()
    {
        RegisterRequested?.Invoke(this, EventArgs.Empty);
    }

    [RelayCommand]
    private void ForgotPassword()
    {
        ForgotPasswordRequested?.Invoke(this, EventArgs.Empty);
    }

    public void Reset()
    {
        Email = string.Empty;
        Password = string.Empty;
        RememberMe = false;
        IsLoginSuccessful = false;
        ErrorMessage = null;
    }

    partial void OnEmailChanged(string value)
    {
        ErrorMessage = null;
    }

    partial void OnPasswordChanged(string value)
    {
        ErrorMessage = null;
    }
}

