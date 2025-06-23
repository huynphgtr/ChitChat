using FluentValidation;
using ChitChatApp.Share.ViewModels.Main;

namespace ChitChatApp.Share.Validators;

public class ContactListViewModelValidator : AbstractValidator<ContactListViewModel>
{
    public ContactListViewModelValidator()
    {
        RuleFor(x => x.SearchText)
            .MaximumLength(100)
            .WithMessage("Search text must not exceed 100 characters")
            .Matches(@"^[a-zA-Z0-9\s@._-]*$")
            .When(x => !string.IsNullOrEmpty(x.SearchText))
            .WithMessage("Search text contains invalid characters");
    }
}

