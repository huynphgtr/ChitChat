using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FluentValidation;
using FluentValidation.Results;
using ChitChatApp.Share.Validators;

namespace ChitChatApp.Share.ViewModels.Base;

public abstract partial class BaseViewModel : ObservableObject, IDisposable, IValidatableViewModel
{
    [ObservableProperty]
    private bool isBusy;

    [ObservableProperty]
    private string? title;

    [ObservableProperty]
    private string? errorMessage;

    [ObservableProperty]
    private ValidationResult validationResult = new();

    [ObservableProperty]
    private bool isValid = true;

    [ObservableProperty]
    private string validationError = string.Empty;

    protected IValidator? Validator { get; set; }

    protected virtual async Task ExecuteAsync(Func<Task> operation, bool showBusy = true)
    {
        try
        {
            if (showBusy)
                IsBusy = true;

            ErrorMessage = null;
            await operation();
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
            // Log exception here if needed
        }
        finally
        {
            if (showBusy)
                IsBusy = false;
        }
    }

    protected virtual async Task<T?> ExecuteAsync<T>(Func<Task<T>> operation, bool showBusy = true)
    {
        try
        {
            if (showBusy)
                IsBusy = true;

            ErrorMessage = null;
            return await operation();
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
            // Log exception here if needed
            return default;
        }
        finally
        {
            if (showBusy)
                IsBusy = false;
        }
    }

    public virtual async Task<bool> ValidateAsync()
    {
        if (Validator == null)
        {
            IsValid = true;
            ValidationError = string.Empty;
            return true;
        }

        var context = new ValidationContext<object>(this);
        ValidationResult = await Validator.ValidateAsync(context);
        IsValid = ValidationResult.IsValid;
        
        if (!IsValid)
        {
            ValidationError = string.Join("\n", ValidationResult.Errors.Select(e => e.ErrorMessage));
        }
        else
        {
            ValidationError = string.Empty;
        }

        return IsValid;
    }

    public virtual void ClearValidation()
    {
        ValidationResult = new ValidationResult();
        IsValid = true;
        ValidationError = string.Empty;
    }

    protected virtual void Dispose(bool disposing)
    {
        // Derived classes can override this method to clean up resources
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}

