using Microsoft.EntityFrameworkCore;
using WiseJourneyBackend.Domain.Entities.Places;
using WiseJourneyBackend.Domain.Repositories;
using WiseJourneyBackend.Infrastructure.Data;

namespace WiseJourneyBackend.Infrastructure.Repositories;

public class PlaceRepository : IPlaceRepository
{
    private readonly AppDbContext _dbContext;

    public PlaceRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddRangeAsync(List<Place> places)
    {
        var existingPlaceIds = await _dbContext.Places
        .Where(p => places.Select(x => x.Id).Contains(p.Id))
        .Select(p => p.Id)
        .ToListAsync();

        var newPlaces = places.Where(p => !existingPlaceIds.Contains(p.Id)).ToList();

        if (newPlaces.Count != 0)
        {
            _dbContext.Places.AddRange(newPlaces);
            await _dbContext.SaveChangesAsync();
        }
    }
}