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
            TripDayId = Guid.NewGuid()
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
        var tripDay = new TripDay { Id = Guid.NewGuid()};
        var tripPlace = new TripPlace { Id = Guid.NewGuid(), TripDay = tripDay, TripDayId = tripDay.Id };

        // Assert
        Assert.Equal(tripDay.Id, tripPlace.TripDayId);
        Assert.Equal(tripDay, tripPlace.TripDay);
    }
}