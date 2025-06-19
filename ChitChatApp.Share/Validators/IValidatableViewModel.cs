using FluentValidation.Results;

namespace ChitChatApp.Share.Validators;

public interface IValidatableViewModel
{
    ValidationResult ValidationResult { get; }
    bool IsValid { get; }
    string ValidationError { get; }
    Task<bool> ValidateAsync();
    void ClearValidation();
}

