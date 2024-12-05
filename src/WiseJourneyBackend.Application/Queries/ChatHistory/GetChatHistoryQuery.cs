using WiseJourneyBackend.Application.Cache;
using WiseJourneyBackend.Application.Interfaces.Messaging;

namespace WiseJourneyBackend.Application.Queries.ChatHistory;

public record GetChatHistoryQuery() : IQuery<ChatHistoryCacheData>;