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
        var model = new CreateTripPlace ("");
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.PlaceId)
              .WithErrorMessage("Place ID is required.");
    }


    [Fact]
    public void ShouldNotHaveValidationErrorFor_ValidPlaceId()
    {
        var model = new CreateTripPlace ("Place1");
        var result = _validator.TestValidate(model);
        result.ShouldNotHaveValidationErrorFor(x => x.PlaceId);
    }
}