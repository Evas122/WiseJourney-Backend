using MediatR;
using WiseJourneyBackend.Application.Interfaces.Messaging;

namespace WiseJourneyBackend.Application.Commands.DeleteTrip;

public record DeleteTripCommand(Guid Id) : ICommand<Unit>;