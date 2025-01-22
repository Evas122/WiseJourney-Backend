using FluentValidation.TestHelper;
using WiseJourneyBackend.Application.Commands.SendPreferenceMessage;

namespace WiseJourneyBackend.Application.Tests.ValidatorsTests;

public class SendPreferenceMessageValidatorTests
{
    private readonly SendPreferenceMessageValidator _validator;

    public SendPreferenceMessageValidatorTests()
    {
        _validator = new SendPreferenceMessageValidator();
    }

    [Fact]
    public void ShouldHaveValidationErrorFor_Message_WhenMessageIsEmpty()
    {
        var model = new SendPreferenceMessageCommand(string.Empty);
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Message)
              .WithErrorMessage("Message cannot be empty");
    }

    [Fact]
    public void ShouldHaveValidationErrorFor_Message_WhenMessageIsTooShort()
    {
        var model = new SendPreferenceMessageCommand("Hi");
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Message)
              .WithErrorMessage("Message must be at least 3 characters long.");
    }

    [Fact]
    public void ShouldHaveValidationErrorFor_Message_WhenMessageIsTooLong()
    {
        var longMessage = new string('a', 501);
        var model = new SendPreferenceMessageCommand(longMessage);
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Message)
              .WithErrorMessage("Message cannot exceed 500 characters.");
    }

    [Fact]
    public void ShouldNotHaveValidationErrorFor_ValidMessage()
    {
        var model = new SendPreferenceMessageCommand("Valid message");
        var result = _validator.TestValidate(model);
        result.ShouldNotHaveValidationErrorFor(x => x.Message);
    }

    [Fact]
    public void ShouldHaveValidationErrorFor_Message_WhenMessageIsNull()
    {
        var model = new SendPreferenceMessageCommand(string.Empty);
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Message)
              .WithErrorMessage("Message cannot be empty");
    }
}