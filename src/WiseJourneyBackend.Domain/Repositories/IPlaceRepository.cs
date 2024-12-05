using WiseJourneyBackend.Domain.Entities.Places;

namespace WiseJourneyBackend.Domain.Repositories;

public interface IPlaceRepository
{
    Task AddRangeAsync(List<Place> places);
}