using MediatR;
using WiseJourneyBackend.Application.Interfaces.Messaging;
using WiseJourneyBackend.Domain.Entities.Trips;
using WiseJourneyBackend.Domain.Exceptions;
using WiseJourneyBackend.Domain.Repositories;

namespace WiseJourneyBackend.Application.Commands.DeleteTrip;

public class DeleteTripHandler : ICommandHandler<DeleteTripCommand, Unit>
{
    private readonly ITripRepository _tripRepository;

    public DeleteTripHandler(ITripRepository tripRepository)
    {
        _tripRepository = tripRepository;
    }

    public async Task<Unit> Handle(DeleteTripCommand command, CancellationToken cancellationToken)
    {
        var trip = await _tripRepository.GetTripByIdAsync(command.Id) ?? throw new NotFoundException(nameof(Trip), command.Id.ToString());

        await _tripRepository.DeleteTripAsync(trip);

        return Unit.Value;
    }
}