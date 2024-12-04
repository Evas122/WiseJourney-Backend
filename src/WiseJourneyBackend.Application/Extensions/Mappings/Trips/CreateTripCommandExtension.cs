using WiseJourneyBackend.Application.Commands.CreateTrip;
using WiseJourneyBackend.Domain.Entities.Trips;

namespace WiseJourneyBackend.Application.Extensions.Mappings.Trips;

public static class CreateTripCommandExtension
{
    public static Trip ToEntity(this CreateTripCommand command, Guid userId)
    {
        var trip = new Trip
        {
            Id = Guid.NewGuid(),
            Name = command.Name,
            StartDateUtc = command.StartDateUtc,
            EndDateUtc = command.EndDateUtc,
            UserId = userId,
            TripDays = command.TripDays.Select(td => new TripDay
            {
                Id = Guid.NewGuid(),
                DateUtc = td.DateUtc,
                TripPlaces = td.TripPlaces.Select(tp => new TripPlace
                {
                    Id = Guid.NewGuid(),
                    PlaceId = tp.PlaceId,
                    ScheduleTimeUtc = tp.ScheduleTimeUtc
                }).ToList()
            }).ToList()
        };
        return trip;
    }
}