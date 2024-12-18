using Microsoft.EntityFrameworkCore;
using Moq;
using WiseJourneyBackend.Application.Interfaces;
using WiseJourneyBackend.Domain.Entities.Auth;
using WiseJourneyBackend.Infrastructure.Data;
using WiseJourneyBackend.Infrastructure.Repositories;

namespace WiseJourneyBackend.Infrastructure.Tests.RepositoriesTests;

public class RefreshTokenRepositoryTests
{
    private readonly IDateTimeProvider _dateTimeProviderMock;
    private readonly AppDbContext _dbContext;
    private readonly RefreshTokenRepository _repository;

    public RefreshTokenRepositoryTests()
    {
        _dateTimeProviderMock = Mock.Of<IDateTimeProvider>();
        Mock.Get(_dateTimeProviderMock).Setup(dp => dp.UtcNow).Returns(DateTime.UtcNow);

        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _dbContext = new AppDbContext(options);

        _repository = new RefreshTokenRepository(_dbContext, _dateTimeProviderMock);
    }

    [Fact]
    public async Task GetRefreshTokenByUserIdAsync_ShouldReturnToken_WhenExistsAndValid()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var validToken = new RefreshToken
        {
            UserId = userId,
            Token = "valid-token",
            ExpiresAtUtc = DateTime.UtcNow.AddDays(1),
            IsRevoked = false
        };

        _dbContext.RefreshTokens.Add(validToken);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _repository.GetRefreshTokenByUserIdAsync(userId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(validToken.Token, result.Token);
    }

    [Fact]
    public async Task GetRefreshTokenByUserIdAsync_ShouldReturnNull_WhenTokenDoesNotExist()
    {
        // Arrange
        var userId = Guid.NewGuid();

        // Act
        var result = await _repository.GetRefreshTokenByUserIdAsync(userId);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task AddAsync_ShouldAddToken_WhenValidTokenProvided()
    {
        // Arrange
        var refreshToken = new RefreshToken
        {
            Id = Guid.NewGuid(),
            Token = "new-token",
            UserId = Guid.NewGuid(),
            ExpiresAtUtc = DateTime.UtcNow.AddDays(1),
            CreatedAtUtc = DateTime.UtcNow,
            IsRevoked = false
        };

        // Act
        await _repository.AddAsync(refreshToken);

        // Assert
        var addedToken = await _dbContext.RefreshTokens.FindAsync(refreshToken.Id);
        Assert.NotNull(addedToken);
        Assert.Equal(refreshToken.Token, addedToken?.Token);
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateToken_WhenValidTokenProvided()
    {
        // Arrange
        var refreshToken = new RefreshToken
        {
            Id = Guid.NewGuid(),
            Token = "updated-token",
            UserId = Guid.NewGuid(),
            ExpiresAtUtc = DateTime.UtcNow.AddDays(1),
            CreatedAtUtc = DateTime.UtcNow,
            IsRevoked = false
        };

        _dbContext.RefreshTokens.Add(refreshToken);
        await _dbContext.SaveChangesAsync();

        // Act
        refreshToken.Token = "updated-token-new";
        await _repository.UpdateAsync(refreshToken);

        // Assert
        var updatedToken = await _dbContext.RefreshTokens.FindAsync(refreshToken.Id);
        Assert.NotNull(updatedToken);
        Assert.Equal("updated-token-new", updatedToken?.Token);
    }

    [Fact]
    public async Task GetRefreshTokenByTokenAsync_ShouldReturnToken_WhenValidTokenProvided()
    {
        // Arrange
        var token = "valid-token";
        var refreshToken = new RefreshToken
        {
            Token = token,
            UserId = Guid.NewGuid(),
            ExpiresAtUtc = DateTime.UtcNow.AddDays(1),
            CreatedAtUtc = DateTime.UtcNow,
            IsRevoked = false
        };

        _dbContext.RefreshTokens.Add(refreshToken);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _repository.GetRefreshTokenByTokenAsync(token);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(token, result.Token);
    }

    [Fact]
    public async Task GetRefreshTokenByTokenAsync_ShouldReturnNull_WhenTokenNotFound()
    {
        // Arrange
        var token = "non-existent-token";

        // Act
        var result = await _repository.GetRefreshTokenByTokenAsync(token);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetAllActiveTokensAsync_ShouldReturnActiveTokens_WhenExist()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var activeToken = new RefreshToken
        {
            UserId = userId,
            Token = "active-token",
            ExpiresAtUtc = DateTime.UtcNow.AddDays(1),
            IsRevoked = false
        };

        _dbContext.RefreshTokens.Add(activeToken);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _repository.GetAllActiveTokensAsync(userId);

        // Assert
        Assert.NotEmpty(result);
        Assert.Single(result);
        Assert.Equal("active-token", result[0].Token);
    }

    [Fact]
    public async Task RemoveUserRefreshTokensAsync_ShouldRemoveTokens_WhenTokensExist()
    {
        // Arrange
        var refreshToken = new RefreshToken
        {
            Id = Guid.NewGuid(),
            Token = "token-to-remove",
            UserId = Guid.NewGuid(),
            ExpiresAtUtc = DateTime.UtcNow.AddDays(1),
            CreatedAtUtc = DateTime.UtcNow,
            IsRevoked = false
        };

        _dbContext.RefreshTokens.Add(refreshToken);
        await _dbContext.SaveChangesAsync();

        // Act
        await _repository.RemoveUserRefreshTokensAsync(new List<RefreshToken> { refreshToken });

        // Assert
        var removedToken = await _dbContext.RefreshTokens.FindAsync(refreshToken.Id);
        Assert.Null(removedToken);
    }

    [Fact]
    public async Task GetRefreshTokenByUserIdAsync_ShouldReturnNull_WhenTokenIsExpired()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var expiredToken = new RefreshToken
        {
            UserId = userId,
            Token = "expired-token",
            ExpiresAtUtc = DateTime.UtcNow.AddDays(-1),
            IsRevoked = false
        };

        _dbContext.RefreshTokens.Add(expiredToken);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _repository.GetRefreshTokenByUserIdAsync(userId);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetRefreshTokenByTokenAsync_ShouldReturnNull_WhenTokenIsExpired()
    {
        // Arrange
        var token = "expired-token";
        var expiredToken = new RefreshToken
        {
            Token = token,
            UserId = Guid.NewGuid(),
            ExpiresAtUtc = DateTime.UtcNow.AddDays(-1),
            IsRevoked = false,
            CreatedAtUtc = DateTime.UtcNow.AddDays(-2)
        };

        _dbContext.RefreshTokens.Add(expiredToken);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _repository.GetRefreshTokenByTokenAsync(token);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetAllActiveTokensAsync_ShouldReturnEmpty_WhenNoActiveTokens()
    {
        // Arrange
        var userId = Guid.NewGuid();

        // Act
        var result = await _repository.GetAllActiveTokensAsync(userId);

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetRefreshTokenByTokenAsync_ShouldReturnNull_WhenTokenDoesNotExist()
    {
        // Arrange
        var token = "nonexistent-token";

        // Act
        var result = await _repository.GetRefreshTokenByTokenAsync(token);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetAllActiveTokensAsync_ShouldReturnOnlyActiveTokens()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var activeToken = new RefreshToken
        {
            UserId = userId,
            Token = "active-token",
            ExpiresAtUtc = DateTime.UtcNow.AddDays(1),
            IsRevoked = false
        };

        var revokedToken = new RefreshToken
        {
            UserId = userId,
            Token = "revoked-token",
            ExpiresAtUtc = DateTime.UtcNow.AddDays(1),
            IsRevoked = true
        };

        _dbContext.RefreshTokens.Add(activeToken);
        _dbContext.RefreshTokens.Add(revokedToken);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _repository.GetAllActiveTokensAsync(userId);

        // Assert
        Assert.Single(result);
        Assert.Equal("active-token", result[0].Token);
    }
}