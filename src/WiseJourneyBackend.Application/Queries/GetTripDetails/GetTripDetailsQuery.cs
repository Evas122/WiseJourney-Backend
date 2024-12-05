using WiseJourneyBackend.Application.Dtos.Trip;
using WiseJourneyBackend.Application.Interfaces.Messaging;

namespace WiseJourneyBackend.Application.Queries.GetTripDetails;

public record GetTripDetailsQuery(Guid Id) : IQuery<TripDetailsDto>;