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
    public void ShouldHaveValidationErrorFor_DateUtc_WhenDateIsEmpty()
    {
        var model = new CreateTripDay(default, new List<CreateTripPlace>());
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.DateUtc)
              .WithErrorMessage("Day date is required.");
    }

    [Fact]
    public void ShouldNotHaveValidationErrorFor_ValidTripPlaces()
    {
        var model = new CreateTripDay(
            DateUtc: DateTime.UtcNow,
            TripPlaces: new List<CreateTripPlace>
            {
                new CreateTripPlace(PlaceId: "Place1", ScheduleTimeUtc: DateTime.UtcNow)
            }
        );
        var result = _validator.TestValidate(model);
        result.ShouldNotHaveValidationErrorFor(x => x.TripPlaces);
    }
}