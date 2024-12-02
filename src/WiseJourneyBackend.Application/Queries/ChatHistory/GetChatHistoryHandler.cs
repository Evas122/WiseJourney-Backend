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

    public Task<ChatHistoryCacheData> Handle(GetChatHistoryQuery query, CancellationToken cancellationToken)
    {
        var chatHistory =  _recommendationService.GetChatHistoryCacheData();

        return chatHistory;
    }
}