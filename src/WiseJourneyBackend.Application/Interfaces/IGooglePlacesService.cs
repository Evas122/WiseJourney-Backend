using WiseJourneyBackend.Application.Dtos.Places;

namespace WiseJourneyBackend.Application.Interfaces;
public interface IGooglePlacesService
{
    Task <List<PlaceDto>> GetNearbyPlaces(string address);
}