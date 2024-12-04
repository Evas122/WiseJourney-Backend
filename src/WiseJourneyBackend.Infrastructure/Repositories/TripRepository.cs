using Microsoft.EntityFrameworkCore;
using WiseJourneyBackend.Domain.Entities.Trips;
using WiseJourneyBackend.Domain.Repositories;
using WiseJourneyBackend.Infrastructure.Data;

namespace WiseJourneyBackend.Infrastructure.Repositories;

public class TripRepository : ITripRepository
{
    private readonly AppDbContext _dbContext;

    public TripRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddTripAsync(Trip trip)
    {
        _dbContext.Add(trip);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateTripAsync(Trip trip)
    {
        _dbContext.Update(trip);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteTripAsync(Trip trip)
    {
        _dbContext.Trips.Remove(trip);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<(IEnumerable<Trip> Items, int TotalItems)> GetUserTripsAsync(Guid userId, int pageNumber, int pageSize)
    {
        var query = _dbContext.Trips
        .Where(t => t.UserId == userId)
        .OrderByDescending(t => t.CreatedAtUtc);

        var totalItems = await query.CountAsync();
        var items = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalItems);
    }

    public async Task<Trip?> GetTripDetailsByIdAsync(Guid tripId, Guid userId)
    {
        return await _dbContext.Trips
        .AsNoTracking()
        .Include(t => t.TripDays)
            .ThenInclude(td => td.TripPlaces)
                .ThenInclude(tp => tp.Place)
                    .ThenInclude(p => p.Geometry)
        .Include(t => t.TripDays)
            .ThenInclude(td => td.TripPlaces)
                .ThenInclude(tp => tp.Place)
                    .ThenInclude(p => p.OpeningHour)
                        .ThenInclude(oh => oh.WeeklyHours)
        .Include(t => t.TripDays)
            .ThenInclude(td => td.TripPlaces)
                .ThenInclude(tp => tp.Place)
                    .ThenInclude(p => p.PlaceTypes)
        .FirstOrDefaultAsync(x => x.Id == tripId && x.UserId == userId);
    }
}