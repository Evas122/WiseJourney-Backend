using FluentValidation;

namespace WiseJourneyBackend.Application.Commands.SendPreferenceMessage;

public class SendPreferenceMessageValidator : AbstractValidator<SendPreferenceMessageCommand>
{
    public SendPreferenceMessageValidator()
    {
        RuleFor(x => x.Message)
            .NotEmpty()
            .WithMessage("Message cannot be empty")
            .MinimumLength(3)
            .WithMessage("Message must be at least 3 characters long.")
            .MaximumLength(500)
            .WithMessage("Message cannot exceed 500 characters.");
    }
}