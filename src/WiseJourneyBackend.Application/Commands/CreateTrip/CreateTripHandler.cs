using MediatR;
using Microsoft.AspNetCore.Http;
using WiseJourneyBackend.Application.Extensions;
using WiseJourneyBackend.Application.Extensions.Mappings.Trips;
using WiseJourneyBackend.Application.Interfaces.Messaging;
using WiseJourneyBackend.Domain.Repositories;

namespace WiseJourneyBackend.Application.Commands.CreateTrip;

public class CreateTripHandler : ICommandHandler<CreateTripCommand, Unit>
{
    private readonly ITripRepository _tripRepository;
    private readonly IHttpContextAccessor _contextAccessor;

    public CreateTripHandler(ITripRepository tripRepository, IHttpContextAccessor contextAccessor)
    {
        _tripRepository = tripRepository;
        _contextAccessor = contextAccessor;
    }

    public async Task<Unit> Handle(CreateTripCommand command, CancellationToken cancellationToken)
    {
        var userId = _contextAccessor.GetUserId();
        var trip  = command.ToEntity(userId);

        await _tripRepository.AddTripAsync(trip);

        return Unit.Value;
    }
}