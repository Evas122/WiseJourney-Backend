using FluentValidation;

namespace WiseJourneyBackend.Application.Commands.CreateTrip;

public class CreateTripValidator : AbstractValidator<CreateTripCommand>
{
    public CreateTripValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Trip name is required.");

        RuleFor(x => x.TripDays)
            .NotEmpty().WithMessage("Trip must have at least one day.");
        
        RuleForEach(x => x.TripDays)
            .SetValidator(new CreateTripDayValidator());
    }

    public class CreateTripDayValidator : AbstractValidator<CreateTripDay>
    {
        public CreateTripDayValidator()
        {

            RuleForEach(x => x.TripPlaces)
                .SetValidator(new CreateTripPlaceValidator());
        }
    }

    public class CreateTripPlaceValidator : AbstractValidator<CreateTripPlace>
    {
        public CreateTripPlaceValidator()
        {
            RuleFor(x => x.PlaceId)
                .NotEmpty().WithMessage("Place ID is required.");

        }
    }
}