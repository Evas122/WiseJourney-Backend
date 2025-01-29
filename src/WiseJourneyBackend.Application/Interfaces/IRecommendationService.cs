using WiseJourneyBackend.Application.Commands.GeneratePlaces;
using WiseJourneyBackend.Application.Dtos.Places;

namespace WiseJourneyBackend.Application.Interfaces;

public interface IRecommendationService
{
    Task<List<PlaceDto>> GetRecommendedPlacesAsync(GeneratePlacesCommand command);
}