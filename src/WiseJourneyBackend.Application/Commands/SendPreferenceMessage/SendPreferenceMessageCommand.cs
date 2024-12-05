using MediatR;
using WiseJourneyBackend.Application.Interfaces.Messaging;

namespace WiseJourneyBackend.Application.Commands.SendPreferenceMessage;

public record SendPreferenceMessageCommand(string Message) : ICommand<Unit>;