using WiseJourneyBackend.Application.Cache;
using WiseJourneyBackend.Application.Commands.SendPreferenceMessage;

namespace WiseJourneyBackend.Application.Interfaces;
public interface IRecommendationService
{
    Task SendUserPreferencesMessageAsync(SendPreferenceMessageCommand command);
    ChatHistoryCacheData GetChatHistoryCacheData();
}