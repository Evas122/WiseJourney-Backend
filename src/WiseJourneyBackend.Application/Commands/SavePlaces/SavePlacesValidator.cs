using FluentValidation;

namespace WiseJourneyBackend.Application.Commands.SavePlaces;

public class SavePlacesValidator : AbstractValidator<SavePlacesCommand>
{
    public SavePlacesValidator()
    {
        RuleFor(x => x.Places)
            .NotEmpty()
            .WithMessage("Places list cannot be empty.");

        RuleForEach(x => x.Places)
           .SetValidator(new SavePlaceValidator());
    }

    public class SavePlaceValidator : AbstractValidator<SavePlace>
    {
        public SavePlaceValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Place ID is required.");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Place name is required.");

            RuleFor(x => x.FullAddress)
                .NotEmpty().WithMessage("Full address is required.");

            RuleFor(x => x.ShortAddress)
                .NotEmpty().WithMessage("Short address is required.");

            RuleFor(x => x.Rating)
                .InclusiveBetween(0, 5).WithMessage("Rating must be between 0 and 5.");

            RuleFor(x => x.UserRatingTotal)
                .GreaterThanOrEqualTo(0).WithMessage("User rating total cannot be negative.");

            RuleFor(x => x.PriceLevel)
                .InclusiveBetween(0, 5).WithMessage("Price level must be between 0 and 5.");

            RuleFor(x => x.Geometry)
                .SetValidator(new SaveGeometryValidator());

            RuleFor(x => x.OpeningHour)
                .SetValidator(new SaveOpeningHourValidator())
                .When(x => x.OpeningHour != null);

            RuleForEach(x => x.PlaceTypes)
                .SetValidator(new SavePlaceTypeValidator());
        }
    }

    public class SaveGeometryValidator : AbstractValidator<SaveGeometry>
    {
        public SaveGeometryValidator()
        {
            RuleFor(x => x.Latitude)
                .InclusiveBetween(-90, 90).WithMessage("Latitude must be between -90 and 90.");

            RuleFor(x => x.Longitude)
                .InclusiveBetween(-180, 180).WithMessage("Longitude must be between -180 and 180.");
        }
    }

    public class SaveOpeningHourValidator : AbstractValidator<SaveOpeningHour>
    {
        public SaveOpeningHourValidator()
        {
            RuleFor(x => x.WeeklyHours)
                .NotEmpty().WithMessage("Weekly hours cannot be empty.");

            RuleForEach(x => x.WeeklyHours)
                .SetValidator(new SaveWeeklyHourValidator());
        }
    }

    public class SaveWeeklyHourValidator : AbstractValidator<SaveWeeklyHour>
    {
        public SaveWeeklyHourValidator()
        {
            RuleFor(x => x.Day)
                .IsInEnum().WithMessage("Invalid day of the week.");

            RuleFor(x => x.OpenTime)
                .LessThan(x => x.CloseTime).WithMessage("Open time must be before close time.");
        }
    }

    public class SavePlaceTypeValidator : AbstractValidator<SavePlaceType>
    {
        public SavePlaceTypeValidator()
        {
            RuleFor(x => x.TypeName)
                .NotEmpty().WithMessage("Place type name is required.");
        }
    }
}
