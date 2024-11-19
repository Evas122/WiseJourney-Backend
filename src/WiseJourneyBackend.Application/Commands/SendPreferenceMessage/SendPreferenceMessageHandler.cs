using MediatR;
using WiseJourneyBackend.Application.Interfaces;
using WiseJourneyBackend.Application.Interfaces.Messaging;

namespace WiseJourneyBackend.Application.Commands.SendPreferenceMessage;

public record SendPreferenceMessageCommand(string Message) : ICommand<Unit>;

public class SendPreferenceMessageHandler : ICommandHandler<SendPreferenceMessageCommand, Unit>
{
    private readonly IRecommendationService _recommendationService;

    public SendPreferenceMessageHandler(IRecommendationService recommendationService)
    {
        _recommendationService = recommendationService;
    }

    public async Task<Unit> Handle(SendPreferenceMessageCommand command, CancellationToken cancellationToken)
    {
        await _recommendationService.SendUserPreferencesMessageAsync(command);

        return Unit.Value;
    }
}