using Microsoft.AspNetCore.Http;
using WiseJourneyBackend.Application.Dtos.Paged;
using WiseJourneyBackend.Application.Dtos.Trip;
using WiseJourneyBackend.Application.Extensions;
using WiseJourneyBackend.Application.Extensions.Mappings.Trips;
using WiseJourneyBackend.Application.Interfaces.Messaging;
using WiseJourneyBackend.Domain.Repositories;

namespace WiseJourneyBackend.Application.Queries.GetPagedTrips;

public class GetPagedTripsHandler : IQueryHandler<GetPagedTripsQuery, PagedDto<TripDto>>
{
    private readonly ITripRepository _tripRepository;
    private readonly IHttpContextAccessor _contextAccessor;

    public GetPagedTripsHandler(ITripRepository tripRepository, IHttpContextAccessor contextAccessor)
    {
        _tripRepository = tripRepository;
        _contextAccessor = contextAccessor;
    }

    public async Task<PagedDto<TripDto>> Handle(GetPagedTripsQuery query, CancellationToken cancellationToken)
    {
        var userId = _contextAccessor.GetUserId();
        var page = query.Page ?? 1;
        var pageSize = query.PageSize ?? 10;

        var (trips, totalItems) = await _tripRepository.GetUserTripsAsync(userId, page, pageSize);

        var tripDtos = trips.Select(trip => trip.ToDto()).ToList();

        return new PagedDto<TripDto>(
            tripDtos,
            totalItems,
            pageSize,
            page);
    }
}