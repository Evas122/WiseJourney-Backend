using FluentValidation.TestHelper;
using WiseJourneyBackend.Application.Commands.CreateTrip;

namespace WiseJourneyBackend.Application.Tests.ValidatorsTests;

public class CreateTripDayValidatorTests
{
    private readonly CreateTripValidator.CreateTripDayValidator _validator;

    public CreateTripDayValidatorTests()
    {
        _validator = new CreateTripValidator.CreateTripDayValidator();
    }

    [Fact]
    public void ShouldNotHaveValidationErrorFor_ValidTripPlaces()
    {
        var model = new CreateTripDay(
            Day: 1,
            TripPlaces: new List<CreateTripPlace>
            {
                new CreateTripPlace(PlaceId: "Place1")
            }
        );
        var result = _validator.TestValidate(model);
        result.ShouldNotHaveValidationErrorFor(x => x.TripPlaces);
    }
}