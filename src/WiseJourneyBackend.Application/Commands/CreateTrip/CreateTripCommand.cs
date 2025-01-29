using MediatR;
using WiseJourneyBackend.Application.Interfaces.Messaging;

namespace WiseJourneyBackend.Application.Commands.CreateTrip;

public record CreateTripCommand(
    string Name,
    List<CreateTripDay> TripDays) : ICommand<Unit>;

public record CreateTripDay(
    int Day,
    List<CreateTripPlace> TripPlaces);

public record CreateTripPlace(
    string PlaceId);