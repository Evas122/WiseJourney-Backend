using FluentValidation.TestHelper;
using WiseJourneyBackend.Application.Dtos.Auth;
using WiseJourneyBackend.Application.Validators.Auth;

namespace WiseJourneyBackend.Application.Tests.ValidatorsTests;

public class RegisterDtoValidatorTests
{
    private readonly RegisterDtoValidator _validator;

    public RegisterDtoValidatorTests()
    {
        _validator = new RegisterDtoValidator();
    }

    [Fact]
    public void ShouldHaveValidationErrorFor_Email_WhenEmailIsEmpty()
    {
        var model = new RegisterDto ("", "ValidUser", "ValidPassword123!" );
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Email)
              .WithErrorMessage("Email is required.");
    }

    [Fact]
    public void ShouldHaveValidationErrorFor_Email_WhenEmailIsInvalid()
    {
        var model = new RegisterDto ("invalid-email", "ValidUser", "ValidPassword123!");
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Email)
              .WithErrorMessage("Invalid email format.");
    }

    [Fact]
    public void ShouldHaveValidationErrorFor_UserName_WhenUserNameIsTooShort()
    {
        var model = new RegisterDto ("valid@example.com", "abc", "ValidPassword123!");
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.UserName)
              .WithErrorMessage("Username must be at least 4 characters long.");
    }

    [Fact]
    public void ShouldNotHaveValidationErrorFor_ValidUserName()
    {
        var model = new RegisterDto ("valid@example.com", "ValidUser", "ValidPassword123!");
        var result = _validator.TestValidate(model);
        result.ShouldNotHaveValidationErrorFor(x => x.UserName);
    }

    [Fact]
    public void ShouldHaveValidationErrorFor_Password_WhenPasswordDoesNotMatchCriteria()
    {
        var model = new RegisterDto ("valid@example.com", "ValidUser", "password");
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Password)
              .WithErrorMessage("Password must contain at least one uppercase letter.");
    }
}