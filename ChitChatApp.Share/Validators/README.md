# ChitChatApp Validators

This folder contains FluentValidation validators for the ViewModels in the ChitChatApp.Share project. The validation system follows clean architecture principles by separating validation logic from the ViewModels.

## Overview

The validation implementation includes:

1. **IValidatableViewModel** interface - Provides a contract for ViewModels that support validation
2. **BaseViewModel** enhancements - Includes validation properties and methods
3. **Specific Validators** - Individual validator classes for different ViewModels

## Implemented Validators

### LoginViewModelValidator

Validates the `LoginViewModel` with the following rules:
- **Email**: Required, valid email format, max 254 characters
- **Password**: Required, minimum 6 characters, max 100 characters

### MessageInputViewModelValidator

Validates the `MessageInputViewModel` with the following rules:
- **Text**: Required when no attachment or recording, max 2000 characters, cannot be only whitespace
- **AttachmentFileName**: Required when attachment is present, max 255 characters
- **Content Validation**: At least one input method must be used (text, attachment, or recording)

### ContactListViewModelValidator

Validates the `ContactListViewModel` with the following rules:
- **SearchText**: Max 100 characters, only allows alphanumeric characters, spaces, @, ., _, and -

## Usage

### In ViewModels

```csharp
public class LoginViewModel : BaseViewModel
{
    public LoginViewModel()
    {
        // Initialize the validator in the constructor
        Validator = new LoginViewModelValidator();
    }

    [RelayCommand]
    private async Task LoginAsync()
    {
        // Validate before processing
        if (!await ValidateAsync())
        {
            ErrorMessage = ValidationError;
            return;
        }

        // Proceed with login logic...
    }
}
```

### Validation Properties

All validatable ViewModels inherit these properties from BaseViewModel:
- `ValidationResult` - Complete FluentValidation result
- `IsValid` - Boolean indicating if the model is currently valid
- `ValidationError` - Formatted error message string

### Validation Methods

- `ValidateAsync()` - Performs validation and updates properties
- `ClearValidation()` - Clears all validation state

## Best Practices

1. **Initialize validators in constructors** - Set the `Validator` property in the ViewModel constructor
2. **Validate before actions** - Call `ValidateAsync()` before performing important operations
3. **Clear validation when appropriate** - Use `ClearValidation()` after successful operations
4. **Bind to validation properties** - Use `ValidationError` and `IsValid` for UI feedback

## UI Integration

The validation system is designed to work seamlessly with data binding:

```xml
<!-- Display validation errors -->
<TextBlock Text="{Binding ValidationError}" 
           Foreground="Red"
           IsVisible="{Binding !IsValid}" />

<!-- Enable/disable based on validation -->
<Button Command="{Binding LoginCommand}"
        IsEnabled="{Binding IsValid}" />
```

## Benefits

1. **Separation of Concerns** - Validation logic is separate from ViewModels
2. **Reusable** - Validators can be reused and tested independently
3. **Maintainable** - Easy to modify validation rules without changing ViewModels
4. **Consistent** - All ViewModels follow the same validation pattern
5. **Testable** - Validators can be unit tested independently

## Dependencies

- **FluentValidation** (v12.0.0) - For validation rules and engine
- **CommunityToolkit.Mvvm** (v8.2.2) - For MVVM base classes and attributes

## Future Enhancements

Consider adding:
- Custom validation attributes for common patterns
- Validation rule localization for multiple languages
- Complex cross-property validation rules
- Real-time validation on property changes

