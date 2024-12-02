using WiseJourneyBackend.Application.Dtos.Places;
using WiseJourneyBackend.Application.Dtos.Recommendation;

namespace WiseJourneyBackend.Application.Interfaces;
public interface IGooglePlacesService
{
    Task<List<PlaceDto>> GetNearbyPlacesAsync(GooglePlacesPreferencesDto googlePlacesPreferencesDto);
}