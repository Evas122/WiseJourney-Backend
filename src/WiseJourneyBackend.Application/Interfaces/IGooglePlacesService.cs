using WiseJourneyBackend.Application.Dtos.Places;
using WiseJourneyBackend.Application.Dtos.Recommendation;

namespace WiseJourneyBackend.Application.Interfaces;
public interface IGooglePlacesService
{
    Task<List<PlaceDto>> GetNearbyPlacesAsync(GooglePlacesQuery googlePlacesPreferencesDto);
    Task<byte[]> GetPhotoAsync(string photoId);
}