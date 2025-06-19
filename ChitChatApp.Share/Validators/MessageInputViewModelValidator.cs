using FluentValidation;
using ChitChatApp.Share.ViewModels.Chat;

namespace ChitChatApp.Share.Validators;

public class MessageInputViewModelValidator : AbstractValidator<MessageInputViewModel>
{
    public MessageInputViewModelValidator()
    {
        RuleFor(x => x.Text)
            .NotEmpty()
            .When(x => !x.HasAttachment && !x.IsRecording)
            .WithMessage("Message cannot be empty")
            .MaximumLength(2000)
            .WithMessage("Message must not exceed 2000 characters")
            .Must(NotContainOnlyWhitespace)
            .When(x => !string.IsNullOrEmpty(x.Text))
            .WithMessage("Message cannot contain only whitespace");

        RuleFor(x => x.AttachmentFileName)
            .NotEmpty()
            .When(x => x.HasAttachment)
            .WithMessage("Attachment file name is required when attachment is present")
            .MaximumLength(255)
            .WithMessage("Attachment file name must not exceed 255 characters");

        // Custom rule to ensure at least one input method is used
        RuleFor(x => x)
            .Must(HaveValidContent)
            .WithMessage("Please enter a message, add an attachment, or record audio")
            .WithName("Content");
    }

    private static bool NotContainOnlyWhitespace(string text)
    {
        return !string.IsNullOrWhiteSpace(text);
    }

    private static bool HaveValidContent(MessageInputViewModel viewModel)
    {
        return !string.IsNullOrWhiteSpace(viewModel.Text) || 
               viewModel.HasAttachment || 
               viewModel.IsRecording;
    }
}

