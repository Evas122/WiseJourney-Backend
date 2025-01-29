using Microsoft.EntityFrameworkCore;
using WiseJourneyBackend.Domain.Entities.Places;
using WiseJourneyBackend.Infrastructure.Data;
using WiseJourneyBackend.Infrastructure.Repositories;

namespace WiseJourneyBackend.Infrastructure.Tests.RepositoriesTests;

public class PlaceRepositoryTests : IDisposable
{
    private readonly AppDbContext _dbContext;
    private readonly PlaceRepository _placeRepository;

    public PlaceRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _dbContext = new AppDbContext(options);
        _placeRepository = new PlaceRepository(_dbContext);
    }

    [Fact]
    public async Task AddRangeAsync_ShouldNotAddPlaces_WhenPlacesListIsEmpty()
    {
        // Arrange
        var places = new List<Place>();

        // Act
        await _placeRepository.AddRangeAsync(places);

        // Assert
        var count = await _dbContext.Places.CountAsync();
        Assert.Equal(0, count);
    }

    [Fact]
    public async Task AddRangeAsync_ShouldNotAddPlaces_WhenAllPlacesAlreadyExist()
    {
        // Arrange
        var place = new Place
        {
            Id = "1",
            Name = "Test Place",
            FullAddress = "Test Address",
            ShortAddress = "Test Short Address",
            Rating = 5,
            UserRatingsTotal = 100,
            PriceLevel = 3,
            Geometry = new Geometry { PlaceId = "1", Latitude = 0, Longitude = 0 },
            OpeningHour = new OpeningHour { PlaceId = "1", OpenNow = true },
            PlaceTypes = new List<PlaceType>
            {
                new PlaceType { PlaceId = "1", TypeName = "Restaurant" }
            }
        };

        _dbContext.Places.Add(place);
        await _dbContext.SaveChangesAsync();

        var placesToAdd = new List<Place>
        {
            place
        };

        // Act
        await _placeRepository.AddRangeAsync(placesToAdd);

        // Assert
        var count = await _dbContext.Places.CountAsync();
        Assert.Equal(1, count);
    }

    [Fact]
    public async Task AddRangeAsync_ShouldAddNewPlaces_WhenSomePlacesAreNew()
    {
        // Arrange
        var existingPlace = new Place
        {
            Id = "1",
            Name = "Existing Place",
            FullAddress = "Existing Address",
            ShortAddress = "Existing Short Address",
            Rating = 4,
            UserRatingsTotal = 200,
            PriceLevel = 2,
            Geometry = new Geometry { PlaceId = "1", Latitude = 0, Longitude = 0 },
            OpeningHour = new OpeningHour { PlaceId = "1", OpenNow = false },
            PlaceTypes = new List<PlaceType>
            {
                new PlaceType { PlaceId = "1", TypeName = "Hotel" }
            }
        };

        _dbContext.Places.Add(existingPlace);
        await _dbContext.SaveChangesAsync();

        var newPlace = new Place
        {
            Id = "2",
            Name = "New Place",
            FullAddress = "New Address",
            ShortAddress = "New Short Address",
            Rating = 3,
            UserRatingsTotal = 50,
            PriceLevel = 1,
            Geometry = new Geometry { PlaceId = "2", Latitude = 0, Longitude = 0 },
            OpeningHour = new OpeningHour { PlaceId = "2", OpenNow = true },
            PlaceTypes = new List<PlaceType>
            {
                new PlaceType { PlaceId = "2", TypeName = "Cafe" }
            }
        };

        var placesToAdd = new List<Place> { existingPlace, newPlace };

        // Act
        await _placeRepository.AddRangeAsync(placesToAdd);

        // Assert
        var count = await _dbContext.Places.CountAsync();
        Assert.Equal(2, count);
    }

    [Fact]
    public async Task AddRangeAsync_ShouldAddAllPlaces_WhenAllPlacesAreNew()
    {
        // Arrange
        var newPlace1 = new Place
        {
            Id = "2",
            Name = "New Place 1",
            FullAddress = "New Address 1",
            ShortAddress = "New Short Address 1",
            Rating = 3,
            UserRatingsTotal = 50,
            PriceLevel = 1,
            Geometry = new Geometry { PlaceId = "2", Latitude = 0, Longitude = 0 },
            OpeningHour = new OpeningHour { PlaceId = "2", OpenNow = true },
            PlaceTypes = new List<PlaceType>
            {
                new PlaceType { PlaceId = "2", TypeName = "Cafe" }
            }
        };

        var newPlace2 = new Place
        {
            Id = "3",
            Name = "New Place 2",
            FullAddress = "New Address 2",
            ShortAddress = "New Short Address 2",
            Rating = 4,
            UserRatingsTotal = 100,
            PriceLevel = 2,
            Geometry = new Geometry { PlaceId = "3", Latitude = 0, Longitude = 0 },
            OpeningHour = new OpeningHour { PlaceId = "3", OpenNow = false },
            PlaceTypes = new List<PlaceType>
            {
                new PlaceType { PlaceId = "3", TypeName = "Restaurant" }
            }
        };

        var placesToAdd = new List<Place> { newPlace1, newPlace2 };

        // Act
        await _placeRepository.AddRangeAsync(placesToAdd);

        // Assert
        var count = await _dbContext.Places.CountAsync();
        Assert.Equal(2, count);
    }

    public void Dispose()
    {
        _dbContext.Dispose();
    }
}