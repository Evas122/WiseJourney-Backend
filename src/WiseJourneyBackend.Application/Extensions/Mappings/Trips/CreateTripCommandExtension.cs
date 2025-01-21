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
            UserId = userId,
            TripDays = command.TripDays.Select(td => new TripDay
            {
                Id = Guid.NewGuid(),
                Day = td.Day,
                TripPlaces = td.TripPlaces.Select(tp => new TripPlace
                {
                    Id = Guid.NewGuid(),
                    PlaceId = tp.PlaceId
                }).ToList()
            }).ToList()
        };
        return trip;
    }
}