using WiseJourneyBackend.Domain.Entities.Trips;

namespace WiseJourneyBackend.Domain.Tests.EntitiesTests;

public class TripDayEntityTests
{
    private readonly TripDay _tripDay;

    public TripDayEntityTests()
    {
        _tripDay = new TripDay
        {
            Id = Guid.NewGuid(),
            DateUtc = new DateTime(2024, 5, 2),
            TripId = Guid.NewGuid(),
            TripPlaces = new List<TripPlace>()
        };
    }

    [Fact]
    public void TripDay_ShouldHaveCorrectDate()
    {
        // Arrange
        var tripDay = new TripDay
        {
            Id = Guid.NewGuid(),
            DateUtc = new DateTime(2024, 5, 10)
        };

        // Assert
        Assert.Equal(new DateTime(2024, 5, 10), tripDay.DateUtc);
    }

    [Fact]
    public void TripDay_ShouldBeAssociatedWithTrip()
    {
        // Arrange
        var trip = new Trip { Id = Guid.NewGuid(), Name = "Hiking Adventure" };
        var tripDay = new TripDay { Id = Guid.NewGuid(), Trip = trip, TripId = trip.Id };

        // Assert
        Assert.Equal(trip.Id, tripDay.TripId);
        Assert.Equal(trip, tripDay.Trip);
    }

    [Fact]
    public void TripDay_ShouldInitializeWithEmptyTripPlaces()
    {
        // Arrange
        var tripDay = new TripDay { Id = Guid.NewGuid(), DateUtc = DateTime.UtcNow };

        // Assert
        Assert.Empty(tripDay.TripPlaces);
    }
}