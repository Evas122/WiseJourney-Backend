using MediatR;
using WiseJourneyBackend.Application.Interfaces;
using WiseJourneyBackend.Application.Interfaces.Messaging;
using WiseJourneyBackend.Domain.Exceptions;

namespace WiseJourneyBackend.Application.Commands.SendPreferenceMessage;

public class SendPreferenceMessageHandler : ICommandHandler<SendPreferenceMessageCommand, Unit>
{
    private readonly IRecommendationService _recommendationService;

    public SendPreferenceMessageHandler(IRecommendationService recommendationService)
    {
        _recommendationService = recommendationService;
    }

    public async Task<Unit> Handle(SendPreferenceMessageCommand command, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(command.Message))
        {
            throw new ArgumentIsNullException(nameof(command.Message));
        }

        await _recommendationService.SendUserPreferencesMessageAsync(command);

        return Unit.Value;
    }
}