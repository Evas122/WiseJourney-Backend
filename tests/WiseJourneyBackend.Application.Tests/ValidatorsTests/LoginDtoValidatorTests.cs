using FluentValidation.TestHelper;
using WiseJourneyBackend.Application.Dtos.Auth;
using WiseJourneyBackend.Application.Validators.Auth;

namespace WiseJourneyBackend.Application.Tests.ValidatorsTests;

public class LoginDtoValidatorTests
{
    private readonly LoginDtoValidator _validator;

    public LoginDtoValidatorTests()
    {
        _validator = new LoginDtoValidator();
    }

    [Fact]
    public void ShouldHaveValidationErrorFor_Email_WhenEmailIsEmpty()
    {
        var model = new LoginDto ("", "Password123");
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Email)
              .WithErrorMessage("Email is required.");
    }

    [Fact]
    public void ShouldHaveValidationErrorFor_Email_WhenEmailIsInvalid()
    {
        var model = new LoginDto ("invalid-email", "Password123");
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Email)
              .WithErrorMessage("Invalid email format.");
    }

    [Fact]
    public void ShouldNotHaveValidationErrorFor_ValidEmail()
    {
        var model = new LoginDto ("valid@example.com", "Password123" );
        var result = _validator.TestValidate(model);
        result.ShouldNotHaveValidationErrorFor(x => x.Email);
    }

    [Fact]
    public void ShouldHaveValidationErrorFor_Password_WhenPasswordIsEmpty()
    {
        var model = new LoginDto ("valid@example.com", "");
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Password)
              .WithErrorMessage("Password is required.");
    }

    [Fact]
    public void ShouldNotHaveValidationErrorFor_ValidPassword()
    {
        var model = new LoginDto ("valid@example.com", "Password123");
        var result = _validator.TestValidate(model);
        result.ShouldNotHaveValidationErrorFor(x => x.Password);
    }
}