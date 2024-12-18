using Microsoft.AspNetCore.Http;
using Moq;
using System.Security.Claims;
using WiseJourneyBackend.Application.Queries.GetTripDetails;
using WiseJourneyBackend.Domain.Entities.Trips;
using WiseJourneyBackend.Domain.Exceptions;
using WiseJourneyBackend.Domain.Repositories;

namespace WiseJourneyBackend.Application.Tests.QueriesTests;

public class GetTripDetailsHandlerTests
{
    private readonly Mock<ITripRepository> _tripRepositoryMock;
    private readonly Mock<IHttpContextAccessor> _contextAccessorMock;
    private readonly GetTripDetailsHandler _handler;

    public GetTripDetailsHandlerTests()
    {
        var claimsPrincipalMock = new Mock<ClaimsPrincipal>();
        claimsPrincipalMock.Setup(c => c.FindFirst(ClaimTypes.NameIdentifier))
            .Returns(new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()));

        var httpContextMock = new Mock<HttpContext>();
        httpContextMock.Setup(c => c.User)
            .Returns(claimsPrincipalMock.Object);

        _contextAccessorMock = new Mock<IHttpContextAccessor>();
        _contextAccessorMock.Setup(h => h.HttpContext)
            .Returns(httpContextMock.Object);

        _tripRepositoryMock = new Mock<ITripRepository>();

        _handler = new GetTripDetailsHandler(_tripRepositoryMock.Object, _contextAccessorMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldThrowNotFoundException_WhenTripDoesNotExist()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var tripId = Guid.NewGuid();

        _tripRepositoryMock.Setup(r => r.GetTripDetailsByIdAsync(tripId, userId))
            .ReturnsAsync(default(Trip));

        var query = new GetTripDetailsQuery(tripId);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(query, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_ShouldThrowBadRequestException_WhenUserNotFound()
    {
        // Arrange
        var tripId = Guid.NewGuid();

        var claimsPrincipalMock = new Mock<ClaimsPrincipal>();
        claimsPrincipalMock.Setup(c => c.FindFirst(ClaimTypes.NameIdentifier))
            .Returns(default(Claim));

        var httpContextMock = new Mock<HttpContext>();
        httpContextMock.Setup(c => c.User).Returns(claimsPrincipalMock.Object);

        _contextAccessorMock.Setup(h => h.HttpContext).Returns(httpContextMock.Object);

        _tripRepositoryMock.Setup(r => r.GetTripDetailsByIdAsync(tripId, It.IsAny<Guid>()))
            .ReturnsAsync(default(Trip));

        var query = new GetTripDetailsQuery(tripId);

        // Act & Assert
        await Assert.ThrowsAsync<BadRequestException>(() => _handler.Handle(query, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_ShouldMapTripToDtoCorrectly()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var tripId = Guid.NewGuid();

        var trip = new Trip
        {
            Id = tripId,
            Name = "Trip to Paris",
            StartDateUtc = DateTime.UtcNow,
            EndDateUtc = DateTime.UtcNow.AddDays(7),
            TripDays = new List<TripDay>
        {
            new TripDay
            {
                DateUtc = DateTime.UtcNow,
                TripPlaces = new List<TripPlace>()
            }
        }
        };

        _tripRepositoryMock.Setup(r => r.GetTripDetailsByIdAsync(tripId, userId))
            .ReturnsAsync(trip);

        var claimsPrincipalMock = new Mock<ClaimsPrincipal>();
        claimsPrincipalMock.Setup(c => c.FindFirst(ClaimTypes.NameIdentifier))
            .Returns(new Claim(ClaimTypes.NameIdentifier, userId.ToString()));

        var httpContextMock = new Mock<HttpContext>();
        httpContextMock.Setup(c => c.User)
            .Returns(claimsPrincipalMock.Object);

        var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
        httpContextAccessorMock.Setup(h => h.HttpContext)
            .Returns(httpContextMock.Object);

        var handler = new GetTripDetailsHandler(_tripRepositoryMock.Object, httpContextAccessorMock.Object);

        var query = new GetTripDetailsQuery(tripId);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(trip.Name, result.Name);
        Assert.Equal(trip.StartDateUtc, result.StartDateUtc);
        Assert.Equal(trip.EndDateUtc, result.EndDateUtc);
        Assert.Single(result.TripDays);
    }
}