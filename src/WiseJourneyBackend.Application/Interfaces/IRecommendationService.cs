using WiseJourneyBackend.Application.Cache;
using WiseJourneyBackend.Application.Commands.SendPreferenceMessage;
using WiseJourneyBackend.Application.Dtos.Places;

namespace WiseJourneyBackend.Application.Interfaces;
public interface IRecommendationService
{
    Task SendUserPreferencesMessageAsync(SendPreferenceMessageCommand command);
    Task<ChatHistoryCacheData> GetChatHistoryCacheData();
    Task<List<PlaceDto>> GetRecommendedPlacesAsync();
}