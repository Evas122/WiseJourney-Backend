using MediatR;
using WiseJourneyBackend.Application.Interfaces.Messaging;

namespace WiseJourneyBackend.Application.Commands.CreateTrip;

public record CreateTripCommand(
    string Name,
    DateTime StartDateUtc,
    DateTime EndDateUtc,
    List<CreateTripDay> TripDays) : ICommand<Unit>;

public record CreateTripDay(
    DateTime DateUtc,
    List<CreateTripPlace> TripPlaces);

public record CreateTripPlace(
    string PlaceId,
    DateTime? ScheduleTimeUtc);