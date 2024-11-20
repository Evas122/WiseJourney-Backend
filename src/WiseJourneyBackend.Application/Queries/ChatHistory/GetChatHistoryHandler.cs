using WiseJourneyBackend.Application.Cache;
using WiseJourneyBackend.Application.Interfaces;
using WiseJourneyBackend.Application.Interfaces.Messaging;

namespace WiseJourneyBackend.Application.Queries.ChatHistory;

public record GetChatHistoryQuery() :IQuery<ChatHistoryCacheData>;
public class GetChatHistoryHandler : IQueryHandler<GetChatHistoryQuery, ChatHistoryCacheData>
{
    private readonly IRecommendationService _recommendationService;

    public GetChatHistoryHandler(IRecommendationService recommendationService)
    {
        _recommendationService = recommendationService;
    }

    public async Task<ChatHistoryCacheData> Handle(GetChatHistoryQuery query, CancellationToken cancellationToken)
    {
        var chatHistory = await Task.Run(() => _recommendationService.GetChatHistoryCacheData());

        return chatHistory;
    }
}