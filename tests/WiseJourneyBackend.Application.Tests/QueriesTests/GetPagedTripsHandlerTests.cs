using Microsoft.AspNetCore.Http;
using Moq;
using System.Security.Claims;
using WiseJourneyBackend.Application.Dtos.Trip;
using WiseJourneyBackend.Application.Queries.GetPagedTrips;
using WiseJourneyBackend.Domain.Entities.Trips;
using WiseJourneyBackend.Domain.Repositories;

namespace WiseJourneyBackend.Application.Tests.QueriesTests;

public class GetPagedTripsHandlerTests
{
    private readonly Mock<ITripRepository> _tripRepositoryMock;
    private readonly Mock<IHttpContextAccessor> _contextAccessorMock;
    private readonly GetPagedTripsHandler _handler;

    public GetPagedTripsHandlerTests()
    {
        _tripRepositoryMock = new Mock<ITripRepository>();
        _contextAccessorMock = new Mock<IHttpContextAccessor>();

        _handler = new GetPagedTripsHandler(_tripRepositoryMock.Object, _contextAccessorMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnPagedTrips_WhenValidQueryIsProvided()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var trips = new List<Trip>
    {
        new Trip
        {
            Id = Guid.NewGuid(),
            Name = "Trip 1"
        },
        new Trip
        {
            Id = Guid.NewGuid(),
            Name = "Trip 2"
        }
    };

        var totalItems = 2;
        var page = 1;
        var pageSize = 10;

        var tripDtos = trips.Select(t => new TripDto(
            t.Id,
            t.Name)).ToList();

        var claimsPrincipalMock = new Mock<ClaimsPrincipal>();
        claimsPrincipalMock.Setup(c => c.FindFirst(ClaimTypes.NameIdentifier))
            .Returns(new Claim(ClaimTypes.NameIdentifier, userId.ToString()));

        var httpContextMock = new Mock<HttpContext>();
        httpContextMock.Setup(c => c.User)
            .Returns(claimsPrincipalMock.Object);

        var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
        httpContextAccessorMock.Setup(h => h.HttpContext)
            .Returns(httpContextMock.Object);

        var tripRepositoryMock = new Mock<ITripRepository>();
        tripRepositoryMock.Setup(r => r.GetUserTripsAsync(userId, page, pageSize))
            .ReturnsAsync((trips, totalItems));

        var handler = new GetPagedTripsHandler(tripRepositoryMock.Object, httpContextAccessorMock.Object);

        var query = new GetPagedTripsQuery(page, pageSize);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Items.Count);
        Assert.Equal(totalItems, result.TotalItemsCount);
        Assert.Equal(page, result.ItemsFrom);
        Assert.Equal(pageSize, result.ItemsTo);

        Assert.All(result.Items, item =>
        {
            Assert.Contains(tripDtos, dto => dto.Id == item.Id && dto.Name == item.Name);
        });
    }

    [Fact]
    public async Task Handle_ShouldReturnEmptyList_WhenNoTripsAreFound()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var trips = new List<Trip>();
        var totalItems = 0;
        var page = 1;
        var pageSize = 10;

        var tripDtos = new List<TripDto>();

        var claimsPrincipalMock = new Mock<ClaimsPrincipal>();
        claimsPrincipalMock.Setup(c => c.FindFirst(ClaimTypes.NameIdentifier))
            .Returns(new Claim(ClaimTypes.NameIdentifier, userId.ToString()));

        var httpContextMock = new Mock<HttpContext>();
        httpContextMock.Setup(c => c.User)
            .Returns(claimsPrincipalMock.Object);

        var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
        httpContextAccessorMock.Setup(h => h.HttpContext)
            .Returns(httpContextMock.Object);

        var tripRepositoryMock = new Mock<ITripRepository>();
        tripRepositoryMock.Setup(r => r.GetUserTripsAsync(userId, page, pageSize))
            .ReturnsAsync((trips, totalItems));

        var handler = new GetPagedTripsHandler(tripRepositoryMock.Object, httpContextAccessorMock.Object);

        var query = new GetPagedTripsQuery(page, pageSize);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result.Items);
        Assert.Equal(totalItems, result.TotalItemsCount);
    }

    [Fact]
    public async Task Handle_ShouldReturnCorrectPageAndPageSize_WhenProvided()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var trips = new List<Trip>
    {
        new Trip { Id = Guid.NewGuid(), Name = "Trip 1",},
        new Trip { Id = Guid.NewGuid(), Name = "Trip 2" }
    };
        var totalItems = 2;
        var page = 2;
        var pageSize = 1;

        var tripDtos = trips.Select(t => new TripDto(t.Id, t.Name)).ToList();

        var claimsPrincipalMock = new Mock<ClaimsPrincipal>();
        claimsPrincipalMock.Setup(c => c.FindFirst(ClaimTypes.NameIdentifier))
            .Returns(new Claim(ClaimTypes.NameIdentifier, userId.ToString()));

        var httpContextMock = new Mock<HttpContext>();
        httpContextMock.Setup(c => c.User)
            .Returns(claimsPrincipalMock.Object);

        var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
        httpContextAccessorMock.Setup(h => h.HttpContext)
            .Returns(httpContextMock.Object);

        var tripRepositoryMock = new Mock<ITripRepository>();
        tripRepositoryMock.Setup(r => r.GetUserTripsAsync(userId, page, pageSize))
            .ReturnsAsync((trips.Skip((page - 1) * pageSize).Take(pageSize).ToList(), totalItems));

        var handler = new GetPagedTripsHandler(tripRepositoryMock.Object, httpContextAccessorMock.Object);

        var query = new GetPagedTripsQuery(page, pageSize);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Single(result.Items);
        Assert.Equal(2, result.TotalItemsCount);
        Assert.Equal(2, result.ItemsFrom);
        Assert.Equal(2, result.ItemsTo);
    }

    [Fact]
    public async Task Handle_ShouldReturnFirstPage_WhenPageIsNull()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var trips = new List<Trip>
    {
        new Trip { Id = Guid.NewGuid(), Name = "Trip 1"},
        new Trip { Id = Guid.NewGuid(), Name = "Trip 2"}
    };
        var totalItems = 2;
        var page = 1;
        var pageSize = 10;

        var claimsPrincipalMock = new Mock<ClaimsPrincipal>();
        claimsPrincipalMock.Setup(c => c.FindFirst(ClaimTypes.NameIdentifier))
            .Returns(new Claim(ClaimTypes.NameIdentifier, userId.ToString()));

        var httpContextMock = new Mock<HttpContext>();
        httpContextMock.Setup(c => c.User)
            .Returns(claimsPrincipalMock.Object);

        var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
        httpContextAccessorMock.Setup(h => h.HttpContext)
            .Returns(httpContextMock.Object);

        var tripRepositoryMock = new Mock<ITripRepository>();
        tripRepositoryMock.Setup(r => r.GetUserTripsAsync(userId, page, pageSize))
            .ReturnsAsync((trips, totalItems));

        var handler = new GetPagedTripsHandler(tripRepositoryMock.Object, httpContextAccessorMock.Object);

        var query = new GetPagedTripsQuery(null, pageSize);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Items.Count);
        Assert.Equal(totalItems, result.TotalItemsCount);
        Assert.Equal(1, result.ItemsFrom);
        Assert.Equal(10, result.ItemsTo);
    }

    [Fact]
    public async Task Handle_ShouldReturnCorrectTotalPages_WhenTotalItemsAreGreaterThanPageSize()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var trips = new List<Trip>
    {
        new Trip { Id = Guid.NewGuid(), Name = "Trip 1"},
        new Trip {Id = Guid.NewGuid(), Name = "Trip 2"},
        new Trip {Id = Guid.NewGuid(), Name = "Trip 3"}
    };

        var totalItems = 3;
        var page = 1;
        var pageSize = 2;

        var claimsPrincipalMock = new Mock<ClaimsPrincipal>();
        claimsPrincipalMock.Setup(c => c.FindFirst(ClaimTypes.NameIdentifier))
            .Returns(new Claim(ClaimTypes.NameIdentifier, userId.ToString()));

        var httpContextMock = new Mock<HttpContext>();
        httpContextMock.Setup(c => c.User)
            .Returns(claimsPrincipalMock.Object);

        var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
        httpContextAccessorMock.Setup(h => h.HttpContext)
            .Returns(httpContextMock.Object);

        var tripRepositoryMock = new Mock<ITripRepository>();
        tripRepositoryMock.Setup(r => r.GetUserTripsAsync(userId, page, pageSize))
            .ReturnsAsync((trips.Take(pageSize).ToList(), totalItems));

        var handler = new GetPagedTripsHandler(tripRepositoryMock.Object, httpContextAccessorMock.Object);

        var query = new GetPagedTripsQuery(page, pageSize);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Items.Count);
        Assert.Equal(3, result.TotalItemsCount);
        Assert.Equal(2, result.TotalPages);
    }
}