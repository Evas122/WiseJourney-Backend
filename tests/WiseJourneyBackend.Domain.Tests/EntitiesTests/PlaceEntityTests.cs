using WiseJourneyBackend.Domain.Entities.Places;

namespace WiseJourneyBackend.Domain.Tests.EntitiesTests;

public class PlaceEntityTests
{
    private readonly Place _place;

    public PlaceEntityTests()
    {
        _place = new Place
        {
            Id = "place1",
            Name = "Test Place",
            FullAddress = "123 Main St, Test City",
            ShortAddress = "Test City",
            Rating = 4.5,
            UserRatingsTotal = 100,
            PriceLevel = 2,
            Geometry = new Geometry
            {
                Id = Guid.NewGuid(),
                Latitude = 52.2297,
                Longitude = 21.0122
            }
        };
    }

    [Fact]
    public void Place_ShouldHaveDefaultValues()
    {
        // Assert
        Assert.NotNull(_place.PlaceTypes);
        Assert.NotNull(_place.Geometry);
        Assert.Equal(4.5, _place.Rating);
    }

    [Fact]
    public void Place_ShouldAddPlaceTypeCorrectly()
    {
        // Arrange
        var placeType = new PlaceType
        {
            Id = Guid.NewGuid(),
            PlaceId = _place.Id,
            TypeName = "Restaurant"
        };

        // Act
        _place.PlaceTypes.Add(placeType);

        // Assert
        Assert.Contains(placeType, _place.PlaceTypes);
    }

    [Fact]
    public void Place_ShouldBeLinkedWithGeometry()
    {
        // Arrange
        var geometry = new Geometry
        {
            Id = Guid.NewGuid(),
            PlaceId = _place.Id,
            Latitude = 52.2297,
            Longitude = 21.0122
        };

        // Act
        _place.Geometry = geometry;

        // Assert
        Assert.Equal(geometry, _place.Geometry);
        Assert.Equal(_place.Id, geometry.PlaceId);
    }
}