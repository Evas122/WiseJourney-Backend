using FluentValidation.TestHelper;
using WiseJourneyBackend.Application.Commands.CreateTrip;

namespace WiseJourneyBackend.Application.Tests.ValidatorsTests;

public class CreateTripPlaceValidatorTests
{
    private readonly CreateTripValidator.CreateTripPlaceValidator _validator;

    public CreateTripPlaceValidatorTests()
    {
        _validator = new CreateTripValidator.CreateTripPlaceValidator();
    }

    [Fact]
    public void ShouldHaveValidationErrorFor_PlaceId_WhenPlaceIdIsEmpty()
    {
        var model = new CreateTripPlace ("", DateTime.UtcNow);
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.PlaceId)
              .WithErrorMessage("Place ID is required.");
    }

    [Fact]
    public void ShouldHaveValidationErrorFor_ScheduleTimeUtc_WhenScheduleTimeIsLessThanMinValue()
    {
        var model = new CreateTripPlace ("Place1", DateTime.MinValue);
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.ScheduleTimeUtc)
              .WithErrorMessage("Schedule time must be a valid date.");
    }

    [Fact]
    public void ShouldNotHaveValidationErrorFor_ValidPlaceId()
    {
        var model = new CreateTripPlace ("Place1", DateTime.UtcNow);
        var result = _validator.TestValidate(model);
        result.ShouldNotHaveValidationErrorFor(x => x.PlaceId);
    }

    [Fact]
    public void ShouldNotHaveValidationErrorFor_ValidScheduleTimeUtc()
    {
        var model = new CreateTripPlace ("Place1", DateTime.UtcNow.AddHours(1));
        var result = _validator.TestValidate(model);
        result.ShouldNotHaveValidationErrorFor(x => x.ScheduleTimeUtc);
    }

    [Fact]
    public void ShouldHaveValidationErrorFor_ScheduleTimeUtc_WhenScheduleTimeIsNull()
    {
        var model = new CreateTripPlace ("Place1", null);
        var result = _validator.TestValidate(model);
        result.ShouldNotHaveValidationErrorFor(x => x.ScheduleTimeUtc);
    }
}