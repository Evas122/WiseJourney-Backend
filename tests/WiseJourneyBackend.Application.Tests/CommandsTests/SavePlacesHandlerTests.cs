using Moq;
using WiseJourneyBackend.Application.Commands.SavePlaces;
using WiseJourneyBackend.Domain.Entities.Places;
using WiseJourneyBackend.Domain.Enums;
using WiseJourneyBackend.Domain.Exceptions;
using WiseJourneyBackend.Domain.Repositories;

namespace WiseJourneyBackend.Application.Tests.CommandsTests;

public class SavePlacesHandlerTests
{
    private readonly SavePlacesHandler _handler;
    private readonly Mock<IPlaceRepository> _placeRepositoryMock;

    public SavePlacesHandlerTests()
    {
        _placeRepositoryMock = new Mock<IPlaceRepository>();
        _handler = new SavePlacesHandler(_placeRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldCallAddRangeAsync_WhenPlacesAreValid()
    {
        // Arrange
        var command = new SavePlacesCommand(new List<SavePlace>
        {
            new SavePlace(
                "1",
                "Place 1",
                "Full Address 1",
                "Short Address 1",
                4.5,
                100,
                3,
                new SaveGeometry(52.52, 13.405),
                new SaveOpeningHour(true, new List<SaveWeeklyHour>
                {
                    new SaveWeeklyHour(Day.Monday, DateTime.UtcNow.AddHours(8), DateTime.UtcNow.AddHours(16))
                }),
                new List<SavePlaceType> { new SavePlaceType("Restaurant") })
        });

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _placeRepositoryMock.Verify(x => x.AddRangeAsync(It.IsAny<List<Place>>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldThrowArgumentIsNullException_WhenPlacesIsNull()
    {
        // Arrange
        var command = new SavePlacesCommand(new List<SavePlace>());

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentIsNullException>(() => _handler.Handle(command, CancellationToken.None));
        Assert.Equal("Parameter 'Places' cannot be null or whitespace.", exception.Message);
    }

    [Fact]
    public async Task Handle_ShouldThrowArgumentIsNullException_WhenPlacesIsEmpty()
    {
        // Arrange
        var command = new SavePlacesCommand(new List<SavePlace>());

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentIsNullException>(() => _handler.Handle(command, CancellationToken.None));
        Assert.Equal("Parameter 'Places' cannot be null or whitespace.", exception.Message);
    }

    [Fact]
    public async Task Handle_ShouldThrowBadRequestException_WhenPlaceHasInvalidData()
    {
        // Arrange
        var command = new SavePlacesCommand(new List<SavePlace>
    {
        new SavePlace(
            "1",
            "",
            "Full Address 1",
            "Short Address 1",
            4.5,
            100,
            3,
            new SaveGeometry(52.52, 13.405),
            new SaveOpeningHour(true, new List<SaveWeeklyHour>
            {
                new SaveWeeklyHour(Day.Monday, DateTime.UtcNow.AddHours(8), DateTime.UtcNow.AddHours(16))
            }),
            new List<SavePlaceType> { new SavePlaceType("Restaurant") })
    });

        // Act & Assert
        var exception = await Assert.ThrowsAsync<BadRequestException>(() => _handler.Handle(command, CancellationToken.None));
        Assert.Equal("Place data is invalid.", exception.Message);
    }

    [Fact]
    public async Task Handle_ShouldThrowException_WhenRepositoryFails()
    {
        // Arrange
        var command = new SavePlacesCommand(new List<SavePlace>
    {
        new SavePlace(
            "1",
            "Place 1",
            "Full Address 1",
            "Short Address 1",
            4.5,
            100,
            3,
            new SaveGeometry(52.52, 13.405),
            new SaveOpeningHour(true, new List<SaveWeeklyHour>
            {
                new SaveWeeklyHour(Day.Monday, DateTime.UtcNow.AddHours(8), DateTime.UtcNow.AddHours(16))
            }),
            new List<SavePlaceType> { new SavePlaceType("Restaurant") })
    });

        _placeRepositoryMock
            .Setup(x => x.AddRangeAsync(It.IsAny<List<Place>>()))
            .ThrowsAsync(new Exception("Database error"));

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => _handler.Handle(command, CancellationToken.None));
    }
}