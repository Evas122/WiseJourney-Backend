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
        var model = new CreateTripCommand ( "", new List<CreateTripDay>() );
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Name)
              .WithErrorMessage("Trip name is required.");
    }

    [Fact]
    public void ShouldHaveValidationErrorFor_TripDays_WhenTripDaysAreEmpty()
    {
        var model = new CreateTripCommand ("Trip", new List<CreateTripDay>());
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.TripDays)
              .WithErrorMessage("Trip must have at least one day.");
    }

    [Fact]
    public void ShouldNotHaveValidationErrorFor_ValidTripDays()
    {
        var model = new CreateTripCommand(
            Name: "Trip",
            TripDays: new List<CreateTripDay>
            {
            new CreateTripDay(3, new List <CreateTripPlace>()),
            new CreateTripDay(4, new List<CreateTripPlace>())
            }
        );

        var result = _validator.TestValidate(model);

        result.ShouldNotHaveValidationErrorFor(x => x.TripDays);
    }
}