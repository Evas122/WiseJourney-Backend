using WiseJourneyBackend.Domain.Entities.Trips;

namespace WiseJourneyBackend.Domain.Repositories;

public interface ITripRepository
{
    Task AddTripAsync(Trip trip);
    Task DeleteTripAsync(Trip trip);
    Task <Trip?> GetTripByIdAsync(Guid tripId);
    Task<Trip?> GetTripDetailsByIdAsync(Guid tripId, Guid userId);
    Task<(IEnumerable<Trip> Items, int TotalItems)> GetUserTripsAsync(Guid userId, int pageNumber, int pageSize);
    Task UpdateTripAsync(Trip trip);
}