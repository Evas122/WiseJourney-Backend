using MediatR;
using Microsoft.AspNetCore.Http;
using WiseJourneyBackend.Application.Extensions;
using WiseJourneyBackend.Application.Extensions.Mappings.Trips;
using WiseJourneyBackend.Application.Interfaces;
using WiseJourneyBackend.Application.Interfaces.Messaging;
using WiseJourneyBackend.Domain.Exceptions;
using WiseJourneyBackend.Domain.Repositories;

namespace WiseJourneyBackend.Application.Commands.CreateTrip;

public class CreateTripHandler : ICommandHandler<CreateTripCommand, Unit>
{
    private readonly ITripRepository _tripRepository;
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly IDateTimeProvider _dateTimeProvider;

    public CreateTripHandler(ITripRepository tripRepository, IHttpContextAccessor contextAccessor, IDateTimeProvider dateTimeProvider)
    {
        _tripRepository = tripRepository;
        _contextAccessor = contextAccessor;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<Unit> Handle(CreateTripCommand command, CancellationToken cancellationToken)
    {
        if (command.TripDays == null || command.TripDays.Count == 0)
        {
            throw new ArgumentIsNullException(nameof(command.TripDays));
        }
        var userId = _contextAccessor.GetUserId();
        var trip  = command.ToEntity(userId, _dateTimeProvider);

        await _tripRepository.AddTripAsync(trip);

        return Unit.Value;
    }
}