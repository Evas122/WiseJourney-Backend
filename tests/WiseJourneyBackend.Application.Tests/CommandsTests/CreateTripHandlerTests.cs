using MediatR;
using Microsoft.AspNetCore.Http;
using Moq;
using System.Security.Claims;
using WiseJourneyBackend.Application.Commands.CreateTrip;
using WiseJourneyBackend.Domain.Entities.Trips;
using WiseJourneyBackend.Domain.Exceptions;
using WiseJourneyBackend.Domain.Repositories;

namespace WiseJourneyBackend.Application.Tests.CommandsTests;

public class CreateTripHandlerTests
{
    private readonly CreateTripHandler _handler;
    private readonly Mock<ITripRepository> _tripRepositoryMock;
    private readonly Mock<IHttpContextAccessor> _contextAccessorMock;
    private readonly string _userId;

    public CreateTripHandlerTests()
    {
        _tripRepositoryMock = new Mock<ITripRepository>();
        _contextAccessorMock = new Mock<IHttpContextAccessor>();
        _userId = Guid.NewGuid().ToString();

        var mockHttpContext = new Mock<HttpContext>();
        var claimsIdentity = new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.NameIdentifier, _userId)
        });

        mockHttpContext.Setup(c => c.User).Returns(new ClaimsPrincipal(claimsIdentity));
        _contextAccessorMock.Setup(x => x.HttpContext).Returns(mockHttpContext.Object);

        _handler = new CreateTripHandler(_tripRepositoryMock.Object, _contextAccessorMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldCallAddTripAsync()
    {
        // Arrange
        var command = new CreateTripCommand(
            "Test Trip",

            new List<CreateTripDay>
            {
                new CreateTripDay(1, new List<CreateTripPlace>())
            }
        );

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _tripRepositoryMock.Verify(x => x.AddTripAsync(It.IsAny<Trip>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldNotCallAddTripAsync_WhenCommandIsInvalid()
    {
        // Arrange
        var command = new CreateTripCommand(
            "Test Trip",
            new List<CreateTripDay>()
        );

        // Act
        var exception = await Assert.ThrowsAsync<ArgumentIsNullException>(() => _handler.Handle(command, CancellationToken.None));

        // Assert
        Assert.Equal("Parameter 'TripDays' cannot be null or whitespace.", exception.Message);
        _tripRepositoryMock.Verify(x => x.AddTripAsync(It.IsAny<Trip>()), Times.Never);
    }

    [Fact]
    public async Task Handle_ShouldReturnUnitValue()
    {
        // Arrange
        var command = new CreateTripCommand(
            "Test Trip",
            new List<CreateTripDay>
            {
                new CreateTripDay(2, new List<CreateTripPlace>())
            }
        );

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal(Unit.Value, result);
    }

    [Fact]
    public async Task Handle_ShouldThrowException_WhenRepositoryFails()
    {
        // Arrange
        var command = new CreateTripCommand(
            "Test Trip",
            new List<CreateTripDay>
            {
                new CreateTripDay(3, new List<CreateTripPlace>())
            }
        );

        _tripRepositoryMock
            .Setup(x => x.AddTripAsync(It.IsAny<Trip>()))
            .ThrowsAsync(new Exception("Database error"));

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => _handler.Handle(command, CancellationToken.None));
    }
}