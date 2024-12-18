using WiseJourneyBackend.Domain.Entities.Auth;

namespace WiseJourneyBackend.Domain.Tests.EntitiesTests;

public class RefreshTokenEntityTests
{
    private readonly RefreshToken _refreshToken;

    public RefreshTokenEntityTests()
    {
        _refreshToken = new RefreshToken
        {
            Id = Guid.NewGuid(),
            Token = "token123",
            ExpiresAtUtc = DateTime.UtcNow.AddDays(7),
        };
    }

    [Fact]
    public void RefreshToken_ShouldHaveDefaultValues()
    {
        // Assert
        Assert.False(_refreshToken.IsRevoked);
        Assert.Null(_refreshToken.RevokedAtUtc);
        Assert.NotEqual(default(DateTime), _refreshToken.ExpiresAtUtc);
    }

    [Fact]
    public void RefreshToken_ShouldBeRevokedCorrectly()
    {
        // Act
        _refreshToken.IsRevoked = true;
        _refreshToken.RevokedAtUtc = DateTime.UtcNow;

        // Assert
        Assert.True(_refreshToken.IsRevoked);
        Assert.NotNull(_refreshToken.RevokedAtUtc);
    }

    [Fact]
    public void RefreshToken_ShouldBeAssociatedWithUser()
    {
        // Arrange
        var user = new User
        {
            Id = Guid.NewGuid(),
            UserName = "TestUser"
        };

        var refreshToken = new RefreshToken
        {
            Id = Guid.NewGuid(),
            Token = "token123",
            ExpiresAtUtc = DateTime.UtcNow.AddDays(7),
            User = user,
            UserId = user.Id
        };

        // Assert
        Assert.Equal(user.Id, refreshToken.UserId);
    }
}