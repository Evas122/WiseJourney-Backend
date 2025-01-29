using Microsoft.EntityFrameworkCore;
using Moq;
using System.Linq.Expressions;
using WiseJourneyBackend.Domain.Entities.Auth;
using WiseJourneyBackend.Infrastructure.Data;
using WiseJourneyBackend.Infrastructure.Repositories;

namespace WiseJourneyBackend.Infrastructure.Tests.RepositoriesTests;

public class UserRepositoryTests
{
    private readonly UserRepository _repository;
    private readonly AppDbContext _dbContext;

    public UserRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase("TestDatabase")
            .Options;

        _dbContext = new AppDbContext(options);
        _repository = new UserRepository(_dbContext);
    }

    [Fact]
    public async Task AddAsync_ShouldAddUser()
    {
        // Arrange
        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = "test@example.com",
            EmailConfirmed = false,
            PasswordHash = "dummyhash",
            UserName = "testuser"
        };

        // Act
        await _repository.AddAsync(user);

        // Assert
        var addedUser = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == "test@example.com");
        Assert.NotNull(addedUser);
        Assert.Equal(user.Email, addedUser.Email);
        Assert.Equal(user.PasswordHash, addedUser.PasswordHash);
        Assert.Equal(user.UserName, addedUser.UserName);
    }

    [Fact]
    public async Task IsEmailExistsAsync_ShouldReturnTrue_WhenEmailExists()
    {
        // Arrange
        var email = "test@example.com";
        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = email,
            EmailConfirmed = false,
            PasswordHash = "dummyhash",
            UserName = "testuser"
        };
        await _repository.AddAsync(user);

        // Act
        var result = await _repository.IsEmailExistsAsync(email);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task IsEmailExistsAsync_ShouldReturnFalse_WhenEmailDoesNotExist()
    {
        // Arrange
        var email = "nonexistent@example.com";

        // Act
        var result = await _repository.IsEmailExistsAsync(email);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task IsEmailConfirmedAsync_ShouldReturnTrue_WhenEmailIsConfirmed()
    {
        // Arrange
        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = "test@example.com",
            EmailConfirmed = true,
            PasswordHash = "dummyhash",
            UserName = "testuser"
        };
        await _repository.AddAsync(user);

        // Act
        var result = await _repository.IsEmailConfirmedAsync(user);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task IsEmailConfirmedAsync_ShouldReturnFalse_WhenEmailIsNotConfirmed()
    {
        // Arrange
        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = "test@example.com",
            EmailConfirmed = false,
            PasswordHash = "dummyhash",
            UserName = "testuser"
        };
        await _repository.AddAsync(user);

        // Act
        var result = await _repository.IsEmailConfirmedAsync(user);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task GetByEmailAsync_ShouldReturnUser_WhenEmailExists()
    {
        // Arrange
        var email = "test@example.com";
        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = "test@example.com",
            EmailConfirmed = false,
            PasswordHash = "dummyhash",
            UserName = "testuser"
        };
        await _repository.AddAsync(user);

        // Act
        var result = await _repository.GetByEmailAsync(email);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(email, result.Email);
    }

    [Fact]
    public async Task GetByEmailAsync_ShouldReturnNull_WhenEmailDoesNotExist()
    {
        // Arrange
        var email = "nonexistent@example.com";

        // Act
        var result = await _repository.GetByEmailAsync(email);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnUser_WhenIdExists()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var user = new User
        {
            Id = userId,
            Email = "test@example.com",
            EmailConfirmed = false,
            PasswordHash = "dummyhash",
            UserName = "testuser"
        };
        await _repository.AddAsync(user);

        // Act
        var result = await _repository.GetByIdAsync(userId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(userId, result.Id);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnNull_WhenIdDoesNotExist()
    {
        // Arrange
        var userId = Guid.NewGuid();

        // Act
        var result = await _repository.GetByIdAsync(userId);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task AddAsync_ShouldThrowException_WhenEmailExists()
    {
        // Arrange
        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = "test@example.com",
            EmailConfirmed = false,
            PasswordHash = "dummyhash",
            UserName = "testuser"
        };

        await _repository.AddAsync(user);

        // Act & Assert
        var newUser = new User { Id = Guid.NewGuid(), Email = "test@example.com" };
        await Assert.ThrowsAsync<DbUpdateException>(async () => await _repository.AddAsync(newUser));
    }

    [Fact]
    public async Task AddAsync_ShouldNotThrowException_WhenEmailDoesNotExist()
    {
        // Arrange
        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = "test@example.com",
            EmailConfirmed = false,
            PasswordHash = "dummyhash",
            UserName = "testuser"
        };

        // Act
        await _repository.AddAsync(user);

        // Assert
        var addedUser = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == "test@example.com");
        Assert.NotNull(addedUser);
    }

    [Fact]
    public async Task GetByEmailAsync_ShouldReturnNull_WhenUserDoesNotExist()
    {
        // Arrange
        var email = "nonexistent@example.com";

        // Act
        var result = await _repository.GetByEmailAsync(email);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task IsEmailExistsAsync_ShouldReturnTrue_WhenEmailIsPresentInDb()
    {
        // Arrange
        var email = "existent@example.com";
        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = email,
            EmailConfirmed = false,
            PasswordHash = "dummyhash",
            UserName = "testuser"
        };
        await _repository.AddAsync(user);

        // Act
        var result = await _repository.IsEmailExistsAsync(email);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task IsEmailExistsAsync_ShouldReturnFalse_WhenEmailIsNotInDb()
    {
        // Arrange
        var email = "nonexistent@example.com";

        // Act
        var result = await _repository.IsEmailExistsAsync(email);

        // Assert
        Assert.False(result);
    }
}