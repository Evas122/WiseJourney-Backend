using WiseJourneyBackend.Domain.Entities.Trips;

namespace WiseJourneyBackend.Domain.Tests.EntitiesTests;

public class TripEntityTests
{
    private readonly Trip _trip;

    public TripEntityTests()
    {
        _trip = new Trip
        {
            Id = Guid.NewGuid(),
            Name = "Sample Trip",
            UserId = Guid.NewGuid(),
            TripDays = new List<TripDay>()
        };
    }

    [Fact]
    public void Trip_ShouldInitializeWithEmptyTripDays()
    {
        // Arrange
        var trip = new Trip
        {
            Id = Guid.NewGuid(),
            Name = "Autumn Vacation"
        };

        // Assert
        Assert.Empty(trip.TripDays);
    }

    [Fact]
    public void Trip_ShouldAllowAddingMultipleTripDays()
    {
        // Arrange
        var trip = new Trip { Id = Guid.NewGuid(), Name = "Road Trip" };
        var tripDay1 = new TripDay { Id = Guid.NewGuid(), Trip = trip };
        var tripDay2 = new TripDay { Id = Guid.NewGuid(), Trip = trip };

        // Act
        trip.TripDays.Add(tripDay1);
        trip.TripDays.Add(tripDay2);

        // Assert
        Assert.Equal(2, trip.TripDays.Count);
        Assert.Contains(tripDay1, trip.TripDays);
        Assert.Contains(tripDay2, trip.TripDays);
    }

    [Fact]
    public void Trip_ShouldHaveCorrectName()
    {
        // Arrange
        var trip = new Trip { Id = Guid.NewGuid(), Name = "Family Vacation" };

        // Assert
        Assert.Equal("Family Vacation", trip.Name);
    }

}