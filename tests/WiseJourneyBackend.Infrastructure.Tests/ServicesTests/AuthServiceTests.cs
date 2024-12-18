using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Moq;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using WiseJourneyBackend.Application.Dtos.Auth;
using WiseJourneyBackend.Application.Interfaces;
using WiseJourneyBackend.Domain.Entities.Auth;
using WiseJourneyBackend.Domain.Enums;
using WiseJourneyBackend.Domain.Exceptions;
using WiseJourneyBackend.Domain.Repositories;
using WiseJourneyBackend.Infrastructure.Interfaces;
using WiseJourneyBackend.Infrastructure.Services;

namespace WiseJourneyBackend.Infrastructure.Tests.ServicesTests;

public class AuthServiceTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IRefreshTokenRepository> _refreshTokenRepositoryMock;
    private readonly Mock<IJwtService> _jwtServiceMock;
    private readonly Mock<IPasswordHasher<User>> _passwordHasherMock;
    private readonly Mock<IDateTimeProvider> _dateTimeProviderMock;
    private readonly Mock<IConfiguration> _configurationMock;
    private readonly Mock<IHttpContextAccessor> _contextAccessorMock;
    private readonly Mock<IEmailService> _emailServiceMock;
    private readonly AuthService _authService;

    public AuthServiceTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _refreshTokenRepositoryMock = new Mock<IRefreshTokenRepository>();
        _jwtServiceMock = new Mock<IJwtService>();
        _passwordHasherMock = new Mock<IPasswordHasher<User>>();
        _dateTimeProviderMock = new Mock<IDateTimeProvider>();
        _configurationMock = new Mock<IConfiguration>();
        _contextAccessorMock = new Mock<IHttpContextAccessor>();
        _emailServiceMock = new Mock<IEmailService>();

        _authService = new AuthService(
            _userRepositoryMock.Object,
            _refreshTokenRepositoryMock.Object,
            _jwtServiceMock.Object,
            _passwordHasherMock.Object,
            _dateTimeProviderMock.Object,
            _configurationMock.Object,
            _contextAccessorMock.Object,
            _emailServiceMock.Object
        );
    }

    [Fact]
    public async Task RegisterAsync_EmailExists_ThrowsAlreadyExistsException()
    {
        // Arrange
        var registerDto = new RegisterDto("test@example.com", "TestUser", "SecurePassword123");

        _userRepositoryMock
            .Setup(repo => repo.IsEmailExistsAsync(It.IsAny<string>()))
            .ReturnsAsync(true);

        // Act & Assert
        await Assert.ThrowsAsync<AlreadyExistsException>(() => _authService.RegisterAsync(registerDto));
    }

    [Fact]
    public async Task RegisterAsync_ValidData_CreatesUserAndSendsEmail()
    {
        // Arrange
        var registerDto = new RegisterDto("test@example.com", "TestUser", "SecurePassword123");

        _userRepositoryMock
            .Setup(repo => repo.IsEmailExistsAsync(It.IsAny<string>()))
            .ReturnsAsync(false);

        _jwtServiceMock
            .Setup(jwt => jwt.GenerateEmailVerificationToken(It.IsAny<User>()))
            .Returns("test-token");

        // Act
        await _authService.RegisterAsync(registerDto);

        // Assert
        _userRepositoryMock.Verify(repo => repo.AddAsync(It.Is<User>(u => u.Email == registerDto.Email)), Times.Once);
        _emailServiceMock.Verify(service => service.SendVerificationEmail(It.IsAny<User>(), "test-token"), Times.Once);
    }

    [Fact]
    public async Task LoginAsync_InvalidEmail_ThrowsNotFoundException()
    {
        // Arrange
        var loginDto = new LoginDto("notfound@example.com", "password");

        _userRepositoryMock
            .Setup(repo => repo.GetByEmailAsync(It.IsAny<string>()))
            .ReturnsAsync(default(User));

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _authService.LoginAsync(loginDto));
    }

    [Fact]
    public async Task LoginAsync_InvalidPassword_ThrowsBadRequestException()
    {
        // Arrange
        var loginDto = new LoginDto("test@example.com", "wrongpassword");

        var user = new User { Email = loginDto.Email, PasswordHash = "hashedpassword" };

        _userRepositoryMock
            .Setup(repo => repo.GetByEmailAsync(It.IsAny<string>()))
            .ReturnsAsync(user);

        _passwordHasherMock
            .Setup(hasher => hasher.VerifyHashedPassword(user, "hashedpassword", "wrongpassword"))
            .Returns(PasswordVerificationResult.Failed);

        // Act & Assert
        await Assert.ThrowsAsync<BadRequestException>(() => _authService.LoginAsync(loginDto));
    }

    [Fact]
    public async Task LoginAsync_ValidCredentials_ReturnsAuthResult()
    {
        // Arrange
        var loginDto = new LoginDto("test@example.com", "password");

        var user = new User
        {
            Email = loginDto.Email,
            PasswordHash = "hashedpassword",
            Id = Guid.NewGuid(),
            UserName = "TestUser"
        };

        var claims = new List<Claim>
    {
        new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
        new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        new(ClaimTypes.Name, user.UserName),
        new(ClaimTypes.Email, user.Email)
    };

        _userRepositoryMock
            .Setup(repo => repo.GetByEmailAsync(loginDto.Email))
            .ReturnsAsync(user);

        _passwordHasherMock
            .Setup(hasher => hasher.VerifyHashedPassword(user, user.PasswordHash, loginDto.Password))
            .Returns(PasswordVerificationResult.Success);

        _jwtServiceMock
            .Setup(jwt => jwt.GetClaims(user, null))
            .Returns(claims);

        _jwtServiceMock
            .Setup(jwt => jwt.GenerateJwtToken(claims))
            .Returns("access-token");

        _refreshTokenRepositoryMock
            .Setup(repo => repo.GetRefreshTokenByUserIdAsync(user.Id))
            .ReturnsAsync(new RefreshToken { Token = "refresh-token", Id = Guid.NewGuid(), ExpiresAtUtc = DateTime.UtcNow.AddDays(30) });

        _refreshTokenRepositoryMock
            .Setup(repo => repo.GetRefreshTokenByUserIdAsync(It.Is<Guid>(id => id != user.Id)))
            .ReturnsAsync(default(RefreshToken));

        _refreshTokenRepositoryMock
            .Setup(repo => repo.AddAsync(It.IsAny<RefreshToken>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _authService.LoginAsync(loginDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("access-token", result.AccessToken);
        Assert.Equal("refresh-token", result.RefreshToken);
    }

    [Fact]
    public async Task VerifyEmailAsync_InvalidToken_ReturnsFalse()
    {
        // Arrange
        var invalidToken = "invalid-token";

        _jwtServiceMock
            .Setup(jwt => jwt.ValidateToken(invalidToken))
            .Returns(default(ClaimsPrincipal));

        // Act
        var result = await _authService.VerifyEmailAsync(invalidToken);

        // Assert
        Assert.False(result);
        _userRepositoryMock.Verify(repo => repo.GetByEmailAsync(It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task VerifyEmailAsync_ValidToken_ConfirmsEmail()
    {
        // Arrange
        var validToken = "valid-token";
        var userEmail = "test@example.com";
        var user = new User { Email = userEmail, EmailConfirmed = false };

        var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new[]
        {
        new Claim(ClaimTypes.Email, userEmail),
        new Claim("tokenType", TokenType.EmailConfirmation.ToString())
    }));

        _jwtServiceMock
            .Setup(jwt => jwt.ValidateToken(validToken))
            .Returns(claimsPrincipal);

        _userRepositoryMock
            .Setup(repo => repo.GetByEmailAsync(userEmail))
            .ReturnsAsync(user);

        // Act
        var result = await _authService.VerifyEmailAsync(validToken);

        // Assert
        Assert.True(result);
        Assert.True(user.EmailConfirmed);
        _userRepositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<User>()), Times.Once);
    }

    [Fact]
    public async Task ResetPasswordAsync_InvalidToken_ReturnsFalse()
    {
        // Arrange
        var invalidToken = "invalid-token";
        var newPassword = "NewSecurePassword123!";

        _jwtServiceMock
            .Setup(jwt => jwt.ValidateToken(invalidToken))
            .Returns(default(ClaimsPrincipal));

        // Act
        var result = await _authService.ResetPasswordAsync(invalidToken, newPassword);

        // Assert
        Assert.False(result);
        _userRepositoryMock.Verify(repo => repo.GetByEmailAsync(It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task SendPasswordResetAsync_UserNotFound_ThrowsNotFoundException()
    {
        // Arrange
        var email = "notfound@example.com";

        _userRepositoryMock
            .Setup(repo => repo.GetByEmailAsync(email))
            .ReturnsAsync(default(User));

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _authService.SendPasswordResetAsync(email));
    }

    [Fact]
    public async Task Logout_RemovesAllUserRefreshTokens()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var activeTokens = new List<RefreshToken>
    {
        new RefreshToken { Token = "token1", UserId = userId },
        new RefreshToken { Token = "token2", UserId = userId }
    };

        var claims = new List<Claim>
    {
        new Claim(ClaimTypes.NameIdentifier, userId.ToString()) 
    };
        var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(claims));

        var mockHttpContext = new DefaultHttpContext
        {
            User = claimsPrincipal
        };

        _contextAccessorMock
            .Setup(ctx => ctx.HttpContext)
            .Returns(mockHttpContext);

        _refreshTokenRepositoryMock
            .Setup(repo => repo.GetAllActiveTokensAsync(userId))
            .ReturnsAsync(activeTokens);

        // Act
        await _authService.Logout();

        // Assert
        _refreshTokenRepositoryMock.Verify(repo => repo.RemoveUserRefreshTokensAsync(activeTokens), Times.Once);
    }
}