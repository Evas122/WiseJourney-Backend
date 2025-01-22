using Microsoft.EntityFrameworkCore;
using WiseJourneyBackend.Domain.Entities.Places;
using WiseJourneyBackend.Domain.Entities.Trips;
using WiseJourneyBackend.Domain.Exceptions;
using WiseJourneyBackend.Infrastructure.Data;
using WiseJourneyBackend.Infrastructure.Repositories;

namespace WiseJourneyBackend.Infrastructure.Tests.RepositoriesTests;

public class TripRepositoryTests
{
    private readonly TripRepository _repository;
    private readonly AppDbContext _dbContext;

    public TripRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _dbContext = new AppDbContext(options);
        _repository = new TripRepository(_dbContext);
    }

    [Fact]
    public async Task AddTripAsync_ShouldAddTrip()
    {
        // Arrange
        var trip = new Trip
        {
            Id = Guid.NewGuid(),
            Name = "Test Trip",
            StartDateUtc = DateTime.UtcNow,
            EndDateUtc = DateTime.UtcNow.AddDays(7),
            UserId = Guid.NewGuid(),
            TripDays = new List<TripDay>
        {
            new TripDay
            {
                Id = Guid.NewGuid(),
                DateUtc = DateTime.UtcNow.AddDays(1),
                TripPlaces = new List<TripPlace>
                {
                    new TripPlace
                    {
                        Id = Guid.NewGuid(),
                        PlaceId = "Place1"
                    }
                }
            }
        }
        };

        var place = new Place
        {
            Id = "Place1",
            Name = "Test Place",
            FullAddress = "123 Test Street",
            ShortAddress = "123 St."
        };

        _dbContext.Places.Add(place);
        await _dbContext.SaveChangesAsync();

        // Act
        await _repository.AddTripAsync(trip);

        // Assert
        var addedTrip = await _dbContext.Trips.Include(t => t.TripDays).ThenInclude(td => td.TripPlaces).FirstOrDefaultAsync(t => t.Id == trip.Id);
        Assert.NotNull(addedTrip);
        Assert.Equal(trip.Name, addedTrip.Name);
        Assert.Single(addedTrip.TripDays);
        Assert.Single(addedTrip.TripDays.First().TripPlaces);
    }

    [Fact]
    public async Task UpdateTripAsync_ShouldUpdateTrip()
    {
        // Arrange
        var trip = new Trip
        {
            Id = Guid.NewGuid(),
            Name = "Old Name",
            StartDateUtc = DateTime.UtcNow,
            EndDateUtc = DateTime.UtcNow.AddDays(7),
            UserId = Guid.NewGuid()
        };

        _dbContext.Trips.Add(trip);
        await _dbContext.SaveChangesAsync();

        // Act
        trip.Name = "Updated Name";
        await _repository.UpdateTripAsync(trip);

        // Assert
        var updatedTrip = await _dbContext.Trips.FirstOrDefaultAsync(t => t.Id == trip.Id);
        Assert.NotNull(updatedTrip);
        Assert.Equal("Updated Name", updatedTrip.Name);
    }

    [Fact]
    public async Task DeleteTripAsync_ShouldRemoveTrip()
    {
        // Arrange
        var trip = new Trip
        {
            Id = Guid.NewGuid(),
            Name = "Test Trip",
            StartDateUtc = DateTime.UtcNow,
            EndDateUtc = DateTime.UtcNow.AddDays(7),
            UserId = Guid.NewGuid()
        };

        _dbContext.Trips.Add(trip);
        await _dbContext.SaveChangesAsync();

        // Act
        await _repository.DeleteTripAsync(trip);

        // Assert
        var deletedTrip = await _dbContext.Trips.FirstOrDefaultAsync(t => t.Id == trip.Id);
        Assert.Null(deletedTrip);
    }

    [Fact]
    public async Task GetUserTripsAsync_ShouldReturnUserTrips()
    {
        // Arrange
        var userId = Guid.NewGuid();
        _dbContext.Trips.Add(new Trip { Id = Guid.NewGuid(), Name = "Trip 1", UserId = userId, CreatedAtUtc = DateTime.UtcNow.AddDays(-1) });
        _dbContext.Trips.Add(new Trip { Id = Guid.NewGuid(), Name = "Trip 2", UserId = userId, CreatedAtUtc = DateTime.UtcNow });
        _dbContext.Trips.Add(new Trip { Id = Guid.NewGuid(), Name = "Trip 3", UserId = Guid.NewGuid(), CreatedAtUtc = DateTime.UtcNow });
        await _dbContext.SaveChangesAsync();

        // Act
        var (items, totalItems) = await _repository.GetUserTripsAsync(userId, 1, 2);

        // Assert
        Assert.Equal(2, items.Count());
        Assert.Equal(2, totalItems);
    }

    [Fact]
    public async Task GetTripDetailsByIdAsync_ShouldReturnTripDetails()
    {
        // Arrange
        var place = new Place
        {
            Id = "Place1",
            Name = "Test Place",
            FullAddress = "123 Main St.",
            ShortAddress = "123 St."
        };

        _dbContext.Places.Add(place);

        var trip = new Trip
        {
            Id = Guid.NewGuid(),
            Name = "Test Trip",
            StartDateUtc = DateTime.UtcNow,
            EndDateUtc = DateTime.UtcNow.AddDays(7),
            UserId = Guid.NewGuid(),
            TripDays = new List<TripDay>
        {
            new TripDay
            {
                Id = Guid.NewGuid(),
                DateUtc = DateTime.UtcNow.AddDays(1),
                TripPlaces = new List<TripPlace>
                {
                    new TripPlace
                    {
                        Id = Guid.NewGuid(),
                        PlaceId = "Place1"
                    }
                }
            }
        }
        };

        _dbContext.Trips.Add(trip);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _repository.GetTripDetailsByIdAsync(trip.Id, trip.UserId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Test Trip", result.Name);
        Assert.Equal(trip.StartDateUtc, result.StartDateUtc);
        Assert.Single(result.TripDays);
        var tripDay = result.TripDays.First();
        Assert.Equal(trip.TripDays.First().DateUtc, tripDay.DateUtc);
        Assert.Single(tripDay.TripPlaces);
        var tripPlace = tripDay.TripPlaces.First();
        Assert.Equal("Place1", tripPlace.PlaceId);
        Assert.Equal("Test Place", tripPlace.Place.Name);
        Assert.Equal("123 Main St.", tripPlace.Place.FullAddress);
        Assert.Equal("123 St.", tripPlace.Place.ShortAddress);
    }

    [Fact]
    public async Task GetTripDetailsByIdAsync_ShouldReturnNull_WhenTripNotFound()
    {
        // Arrange
        var tripId = Guid.NewGuid();
        var userId = Guid.NewGuid();

        // Act
        var result = await _repository.GetTripDetailsByIdAsync(tripId, userId);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task AddTripAsync_ShouldThrowException_WhenPlaceNotFound()
    {
        // Arrange
        var trip = new Trip
        {
            Id = Guid.NewGuid(),
            Name = "Test Trip",
            StartDateUtc = DateTime.UtcNow,
            EndDateUtc = DateTime.UtcNow.AddDays(7),
            UserId = Guid.NewGuid(),
            TripDays = new List<TripDay>
            {
                new TripDay
                {
                    Id = Guid.NewGuid(),
                    DateUtc = DateTime.UtcNow.AddDays(1),
                    TripPlaces = new List<TripPlace>
                    {
                        new TripPlace
                        {
                            Id = Guid.NewGuid(),
                            PlaceId = "NonExistentPlace"
                        }
                    }
                }
            }
        };

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(async () => await _repository.AddTripAsync(trip));
    }

    [Fact]
    public async Task AddTripAsync_ShouldLinkExistingPlaceToTripPlace()
    {
        // Arrange
        var existingPlace = new Place
        {
            Id = "Place1",
            Name = "Existing Place",
            FullAddress = "123 Main St.",
            ShortAddress = "123 St."
        };

        _dbContext.Places.Add(existingPlace);
        await _dbContext.SaveChangesAsync();

        var trip = new Trip
        {
            Id = Guid.NewGuid(),
            Name = "Test Trip",
            StartDateUtc = DateTime.UtcNow,
            EndDateUtc = DateTime.UtcNow.AddDays(7),
            UserId = Guid.NewGuid(),
            TripDays = new List<TripDay>
        {
            new TripDay
            {
                Id = Guid.NewGuid(),
                DateUtc = DateTime.UtcNow.AddDays(1),
                TripPlaces = new List<TripPlace>
                {
                    new TripPlace
                    {
                        Id = Guid.NewGuid(),
                        PlaceId = "Place1"
                    }
                }
            }
        }
        };

        // Act
        await _repository.AddTripAsync(trip);

        // Assert
        var addedTrip = await _dbContext.Trips
            .Include(t => t.TripDays)
            .ThenInclude(td => td.TripPlaces)
            .ThenInclude(tp => tp.Place)
            .FirstOrDefaultAsync(t => t.Id == trip.Id);

        Assert.NotNull(addedTrip);
        Assert.Equal("Test Trip", addedTrip.Name);
        Assert.Single(addedTrip.TripDays);
        Assert.Single(addedTrip.TripDays.First().TripPlaces);
        Assert.Equal("Place1", addedTrip.TripDays.First().TripPlaces.First().Place.Id);
        Assert.Equal("Existing Place", addedTrip.TripDays.First().TripPlaces.First().Place.Name);
    }

    [Fact]
    public async Task GetUserTripsAsync_ShouldRespectPagination()
    {
        // Arrange
        var userId = Guid.NewGuid();
        for (int i = 0; i < 10; i++)
        {
            _dbContext.Trips.Add(new Trip
            {
                Id = Guid.NewGuid(),
                Name = $"Trip {i + 1}",
                UserId = userId,
                CreatedAtUtc = DateTime.UtcNow.AddDays(-i)
            });
        }
        await _dbContext.SaveChangesAsync();

        // Act
        var (items, totalItems) = await _repository.GetUserTripsAsync(userId, 2, 3);

        // Assert
        Assert.Equal(3, items.Count());
        Assert.Equal(10, totalItems);
        Assert.Contains(items, t => t.Name == "Trip 4");
        Assert.Contains(items, t => t.Name == "Trip 5");
        Assert.Contains(items, t => t.Name == "Trip 6");
    }

    [Fact]
    public async Task GetUserTripsAsync_ShouldReturnEmpty_WhenNoTripsForUser()
    {
        // Arrange
        var userId = Guid.NewGuid();

        // Act
        var (items, totalItems) = await _repository.GetUserTripsAsync(userId, 1, 10);

        // Assert
        Assert.Empty(items);
        Assert.Equal(0, totalItems);
    }

    [Fact]
    public async Task AddTripAsync_ShouldThrowException_WhenPlaceDoesNotExist()
    {
        // Arrange
        var trip = new Trip
        {
            Id = Guid.NewGuid(),
            Name = "Trip Without Place",
            StartDateUtc = DateTime.UtcNow,
            EndDateUtc = DateTime.UtcNow.AddDays(5),
            UserId = Guid.NewGuid(),
            TripDays = new List<TripDay>
        {
            new TripDay
            {
                Id = Guid.NewGuid(),
                DateUtc = DateTime.UtcNow.AddDays(1),
                TripPlaces = new List<TripPlace>
                {
                    new TripPlace
                    {
                        Id = Guid.NewGuid(),
                        PlaceId = "NonExistentPlace"
                    }
                }
            }
        }
        };

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _repository.AddTripAsync(trip));
    }

    [Fact]
    public async Task DeleteTripAsync_ShouldRemoveTrip_WhenNoRelatedEntities()
    {
        // Arrange
        var trip = new Trip
        {
            Id = Guid.NewGuid(),
            Name = "Simple Trip",
            StartDateUtc = DateTime.UtcNow,
            EndDateUtc = DateTime.UtcNow.AddDays(3),
            UserId = Guid.NewGuid()
        };

        _dbContext.Trips.Add(trip);
        await _dbContext.SaveChangesAsync();

        // Act
        await _repository.DeleteTripAsync(trip);

        // Assert
        var deletedTrip = await _dbContext.Trips.FirstOrDefaultAsync(t => t.Id == trip.Id);
        Assert.Null(deletedTrip);
    }
}