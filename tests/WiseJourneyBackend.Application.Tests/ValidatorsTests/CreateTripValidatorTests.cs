using FluentValidation.TestHelper;
using WiseJourneyBackend.Application.Commands.CreateTrip;

namespace WiseJourneyBackend.Application.Tests.ValidatorsTests;

public class CreateTripValidatorTests
{
    private readonly CreateTripValidator _validator;

    public CreateTripValidatorTests()
    {
        _validator = new CreateTripValidator();
    }

    [Fact]
    public void ShouldHaveValidationErrorFor_Name_WhenNameIsEmpty()
    {
        var model = new CreateTripCommand ( "", DateTime.UtcNow, DateTime.UtcNow.AddDays(1), new List<CreateTripDay>() );
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Name)
              .WithErrorMessage("Trip name is required.");
    }

    [Fact]
    public void ShouldHaveValidationErrorFor_StartDateUtc_WhenStartDateIsAfterEndDate()
    {
        var model = new CreateTripCommand ("Trip", DateTime.UtcNow.AddDays(2), DateTime.UtcNow.AddDays(1), new List<CreateTripDay>());
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.StartDateUtc)
              .WithErrorMessage("Start date must be before end date.");
    }

    [Fact]
    public void ShouldHaveValidationErrorFor_TripDays_WhenTripDaysAreEmpty()
    {
        var model = new CreateTripCommand ("Trip", DateTime.UtcNow, DateTime.UtcNow.AddDays(1), new List<CreateTripDay>());
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.TripDays)
              .WithErrorMessage("Trip must have at least one day.");
    }

    [Fact]
    public void ShouldHaveValidationErrorFor_TripDays_WhenThereAreDuplicateDates()
    {
        var model = new CreateTripCommand(
            Name: "Trip",
            StartDateUtc: DateTime.UtcNow,
            EndDateUtc: DateTime.UtcNow.AddDays(5),
            TripDays: new List<CreateTripDay>
            {
            new CreateTripDay(DateTime.UtcNow, new List<CreateTripPlace>()),
            new CreateTripDay(DateTime.UtcNow, new List<CreateTripPlace>())
            }
        );

        var result = _validator.TestValidate(model);

        result.ShouldHaveValidationErrorFor(x => x.TripDays)
              .WithErrorMessage("Trip days must not have duplicate dates.");
    }

    [Fact]
    public void ShouldNotHaveValidationErrorFor_ValidTripDays()
    {
        var model = new CreateTripCommand(
            Name: "Trip",
            StartDateUtc: DateTime.UtcNow,
            EndDateUtc: DateTime.UtcNow.AddDays(5),
            TripDays: new List<CreateTripDay>
            {
            new CreateTripDay(DateUtc: DateTime.UtcNow, new List <CreateTripPlace>()),
            new CreateTripDay(DateUtc: DateTime.UtcNow.AddDays(1), new List<CreateTripPlace>())
            }
        );

        var result = _validator.TestValidate(model);

        result.ShouldNotHaveValidationErrorFor(x => x.TripDays);
    }
}