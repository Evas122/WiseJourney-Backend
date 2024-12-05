using Microsoft.AspNetCore.Http;
using WiseJourneyBackend.Application.Dtos.Trip;
using WiseJourneyBackend.Application.Extensions;
using WiseJourneyBackend.Application.Extensions.Mappings.Trips;
using WiseJourneyBackend.Application.Interfaces.Messaging;
using WiseJourneyBackend.Domain.Entities.Trips;
using WiseJourneyBackend.Domain.Exceptions;
using WiseJourneyBackend.Domain.Repositories;

namespace WiseJourneyBackend.Application.Queries.GetTripDetails;

public class GetTripDetailsHandler : IQueryHandler<GetTripDetailsQuery, TripDetailsDto>
{
    private readonly ITripRepository _tripRepository;
    private readonly IHttpContextAccessor _contextAccessor;

    public GetTripDetailsHandler(ITripRepository tripRepository, IHttpContextAccessor httpContextAccessor)
    {
        _tripRepository = tripRepository;
        _contextAccessor = httpContextAccessor;
    }

    public async Task<TripDetailsDto> Handle(GetTripDetailsQuery query, CancellationToken cancellationToken)
    {
        var userId = _contextAccessor.GetUserId();
        var tripDetails = await _tripRepository.GetTripDetailsByIdAsync(query.Id, userId)
            ?? throw new NotFoundException(nameof(Trip), query.Id.ToString());

        var tripDetailsDto = tripDetails.ToDetailsDto();

        return tripDetailsDto;
    }
}