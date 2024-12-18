using WiseJourneyBackend.Domain.Entities.Places;
using WiseJourneyBackend.Domain.Entities.Trips;

namespace WiseJourneyBackend.Domain.Tests.EntitiesTests;

public class TripPlaceEntityTests
{
    private readonly TripPlace _tripPlace;

    public TripPlaceEntityTests()
    {
        _tripPlace = new TripPlace
        {
            Id = Guid.NewGuid(),
            PlaceId = "SamplePlaceId",
            TripDayId = Guid.NewGuid(),
            ScheduleTimeUtc = new DateTime(2024, 5, 3, 14, 0, 0)
        };
    }

    [Fact]
    public void TripPlace_ShouldBeAssociatedWithPlace()
    {
        // Arrange
        var place = new Place { Id = "Place123", Name = "Eiffel Tower" };
        var tripPlace = new TripPlace { Id = Guid.NewGuid(), Place = place, PlaceId = place.Id };

        // Assert
        Assert.Equal(place.Id, tripPlace.PlaceId);
        Assert.Equal(place, tripPlace.Place);
    }

    [Fact]
    public void TripPlace_ShouldBeAssociatedWithTripDay()
    {
        // Arrange
        var tripDay = new TripDay { Id = Guid.NewGuid(), DateUtc = new DateTime(2024, 5, 10) };
        var tripPlace = new TripPlace { Id = Guid.NewGuid(), TripDay = tripDay, TripDayId = tripDay.Id };

        // Assert
        Assert.Equal(tripDay.Id, tripPlace.TripDayId);
        Assert.Equal(tripDay, tripPlace.TripDay);
    }

    [Fact]
    public void TripPlace_ShouldStoreScheduleTime()
    {
        // Arrange
        var scheduleTime = new DateTime(2024, 5, 10, 14, 0, 0);
        var tripPlace = new TripPlace { Id = Guid.NewGuid(), ScheduleTimeUtc = scheduleTime };

        // Assert
        Assert.Equal(scheduleTime, tripPlace.ScheduleTimeUtc);
    }
}