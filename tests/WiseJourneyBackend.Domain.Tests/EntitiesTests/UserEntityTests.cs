using WiseJourneyBackend.Domain.Entities.Auth;
using WiseJourneyBackend.Domain.Entities.Trips;

namespace WiseJourneyBackend.Domain.Tests.EntitiesTests;

public class UserEntityTests
{
    private readonly User _user;

    public UserEntityTests()
    {
        _user = new User
        {
            Id = Guid.NewGuid(),
            UserName = "TestUser",
            Email = "test@example.com",
            EmailConfirmed = false,
        };
    }

    [Fact]
    public void User_ShouldHaveDefaultValues()
    {
        // Assert
        Assert.NotNull(_user.RefreshTokens);
        Assert.NotNull(_user.Trips);
        Assert.False(_user.EmailConfirmed);
        Assert.NotEmpty(_user.UserName);
    }

    [Fact]
    public void User_ShouldAddTripCorrectly()
    {
        // Arrange
        var trip = new Trip
        {
            Id = Guid.NewGuid(),
            UserId = _user.Id,
            Name = "Vacation"
        };

        // Act
        _user.Trips.Add(trip);

        // Assert
        Assert.Contains(trip, _user.Trips);
    }

    [Fact]
    public void User_ShouldAddRefreshTokenCorrectly()
    {
        // Arrange
        var refreshToken = new RefreshToken
        {
            Id = Guid.NewGuid(),
            Token = "token123",
            UserId = _user.Id,
            ExpiresAtUtc = DateTime.UtcNow.AddDays(7)
        };

        // Act
        _user.RefreshTokens.Add(refreshToken);

        // Assert
        Assert.Contains(refreshToken, _user.RefreshTokens);
    }
}
