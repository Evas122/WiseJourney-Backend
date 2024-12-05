using WiseJourneyBackend.Application.Dtos.Paged;
using WiseJourneyBackend.Application.Dtos.Trip;
using WiseJourneyBackend.Application.Interfaces.Messaging;

namespace WiseJourneyBackend.Application.Queries.GetPagedTrips;

public record GetPagedTripsQuery(int? Page, int? PageSize) : IQuery<PagedDto<TripDto>>;