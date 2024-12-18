using FluentValidation;

namespace WiseJourneyBackend.Application.Commands.CreateTrip;

public class CreateTripValidator : AbstractValidator<CreateTripCommand>
{
    public CreateTripValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Trip name is required.");

        RuleFor(x => x.StartDateUtc)
            .LessThan(x => x.EndDateUtc).WithMessage("Start date must be before end date.");

        RuleFor(x => x.TripDays)
            .NotEmpty().WithMessage("Trip must have at least one day.")
            .Must(days => days.Select(d => d.DateUtc.Date).Distinct().Count() == days.Count)
            .WithMessage("Trip days must not have duplicate dates.");

        RuleForEach(x => x.TripDays)
            .SetValidator(new CreateTripDayValidator());
    }

    public class CreateTripDayValidator : AbstractValidator<CreateTripDay>
    {
        public CreateTripDayValidator()
        {
            RuleFor(x => x.DateUtc)
                .NotEmpty().WithMessage("Day date is required.");

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

            RuleFor(x => x.ScheduleTimeUtc)
                .GreaterThan(DateTime.MinValue)
                .WithMessage("Schedule time must be a valid date.");
        }
    }
}