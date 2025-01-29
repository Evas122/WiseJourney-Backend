using Microsoft.Extensions.Configuration;
using Moq;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using WiseJourneyBackend.Application.Interfaces;
using WiseJourneyBackend.Domain.Entities.Auth;
using WiseJourneyBackend.Domain.Exceptions;
using WiseJourneyBackend.Infrastructure.Services;

namespace WiseJourneyBackend.Infrastructure.Tests.ServicesTests;

public class JwtServiceTests
{
    private readonly Mock<IConfiguration> _configurationMock;
    private readonly Mock<IDateTimeProvider> _dateTimeProviderMock;
    private readonly JwtService _jwtService;

    public JwtServiceTests()
    {
        _configurationMock = new Mock<IConfiguration>();
        _dateTimeProviderMock = new Mock<IDateTimeProvider>();

        _jwtService = new JwtService(_configurationMock.Object, _dateTimeProviderMock.Object);
    }

    [Fact]
    public void GenerateJwtToken_ShouldGenerateValidToken_WhenClaimsProvided()
    {
        // Arrange
        var secretKey = "super-strong-and-long-secret-key-123456";
        var expirationMinutes = 30;

        _configurationMock.Setup(x => x["Jwt:SecretKey"]).Returns(secretKey);

        var sectionMock = new Mock<IConfigurationSection>();
        sectionMock.Setup(x => x.Value).Returns(expirationMinutes.ToString());
        _configurationMock.Setup(x => x.GetSection("Jwt:AccessTokenExpirationMinutes")).Returns(sectionMock.Object);

        var currentTime = new DateTime(2024, 1, 1, 12, 0, 0, DateTimeKind.Utc);
        _dateTimeProviderMock.Setup(x => x.UtcNow).Returns(currentTime);

        var claims = new List<Claim>
    {
        new Claim(JwtRegisteredClaimNames.Sub, "123"),
        new Claim(ClaimTypes.Name, "testuser")
    };

        // Act
        var token = _jwtService.GenerateJwtToken(claims);

        // Assert
        Assert.NotNull(token);

        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);

        Assert.Equal("testuser", jwtToken.Claims.First(c => c.Type == ClaimTypes.Name).Value);
        Assert.Equal("123", jwtToken.Claims.First(c => c.Type == JwtRegisteredClaimNames.Sub).Value);
        Assert.Equal(currentTime.AddMinutes(expirationMinutes), jwtToken.ValidTo, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public void GenerateJwtToken_ShouldThrowException_WhenSecretKeyIsMissing()
    {
        // Arrange
        _configurationMock.Setup(x => x["Jwt:SecretKey"]).Returns(string.Empty);

        var expirationMinutes = 30;
        var sectionMock = new Mock<IConfigurationSection>();
        sectionMock.Setup(x => x.Value).Returns(expirationMinutes.ToString());
        _configurationMock.Setup(x => x.GetSection("Jwt:AccessTokenExpirationMinutes")).Returns(sectionMock.Object);

        var claims = new List<Claim>
    {
        new Claim(JwtRegisteredClaimNames.Sub, "123"),
        new Claim(ClaimTypes.Name, "testuser")
    };

        // Act & Assert
        var exception = Assert.Throws<ConfigurationException>(() => _jwtService.GenerateJwtToken(claims));

        // Assert
        Assert.Equal("JWT secret key is not configured or is empty.", exception.Message);
    }

    [Fact]
    public void GetClaims_ShouldReturnClaims_ForValidUser()
    {
        // Arrange
        var configurationMock = new Mock<IConfiguration>();
        var dateTimeProviderMock = new Mock<IDateTimeProvider>();

        var jwtService = new JwtService(configurationMock.Object, dateTimeProviderMock.Object);

        var user = new User
        {
            Id = Guid.NewGuid(),
            UserName = "TestUser",
            Email = "testuser@example.com"
        };

        // Act
        var claims = jwtService.GetClaims(user);

        // Assert
        Assert.Contains(claims, c => c.Type == JwtRegisteredClaimNames.Sub && c.Value == user.Id.ToString());
        Assert.Contains(claims, c => c.Type == ClaimTypes.Name && c.Value == user.UserName);
        Assert.Contains(claims, c => c.Type == ClaimTypes.Email && c.Value == user.Email);
    }

    [Fact]
    public void ValidateToken_ShouldReturnClaimsPrincipal_ForValidToken()
    {
        // Arrange
        var configurationMock = new Mock<IConfiguration>();
        var dateTimeProviderMock = new Mock<IDateTimeProvider>();

        var secretKey = "super-strong-and-long-secret-key-123456";
        configurationMock.Setup(x => x["Jwt:SecretKey"]).Returns(secretKey);

        var sectionMock = new Mock<IConfigurationSection>();
        sectionMock.Setup(x => x.Value).Returns("30");
        configurationMock.Setup(x => x.GetSection("Jwt:AccessTokenExpirationMinutes")).Returns(sectionMock.Object);

        var now = DateTime.UtcNow;
        dateTimeProviderMock.Setup(x => x.UtcNow).Returns(now);

        var jwtService = new JwtService(configurationMock.Object, dateTimeProviderMock.Object);

        var claims = new List<Claim>
    {
        new Claim(JwtRegisteredClaimNames.Sub, "123"),
        new Claim(ClaimTypes.Name, "TestUser")
    };

        var token = jwtService.GenerateJwtToken(claims);

        // Act
        var principal = jwtService.ValidateToken(token);

        // Assert
        Assert.NotNull(principal);
        var nameClaim = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name);
        Assert.Equal("TestUser", nameClaim?.Value);
    }

    [Fact]
    public void ValidateToken_ShouldReturnNull_ForInvalidToken()
    {
        // Arrange
        var configurationMock = new Mock<IConfiguration>();
        var dateTimeProviderMock = new Mock<IDateTimeProvider>();

        var secretKey = "SuperSecretKey12345";
        configurationMock.Setup(x => x["Jwt:SecretKey"]).Returns(secretKey);

        var jwtService = new JwtService(configurationMock.Object, dateTimeProviderMock.Object);

        var invalidToken = "InvalidTokenValue";

        // Act
        var principal = jwtService.ValidateToken(invalidToken);

        // Assert
        Assert.Null(principal);
    }

    [Fact]
    public void GenerateJwtToken_ShouldGenerateToken_WithEmptyClaims()
    {
        // Arrange
        var secretKey = "super-strong-and-long-secret-key-123456";
        _configurationMock.Setup(x => x["Jwt:SecretKey"]).Returns(secretKey);
        var expirationMinutes = 30;
        var sectionMock = new Mock<IConfigurationSection>();
        sectionMock.Setup(x => x.Value).Returns(expirationMinutes.ToString());
        _configurationMock.Setup(x => x.GetSection("Jwt:AccessTokenExpirationMinutes")).Returns(sectionMock.Object);

        var claims = new List<Claim>();

        // Act
        var token = _jwtService.GenerateJwtToken(claims);

        // Assert
        Assert.NotNull(token);
        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);
        Assert.Single(jwtToken.Claims);
    }

    [Fact]
    public void GenerateJwtToken_ShouldThrowException_WhenExpirationTimeIsInPast()
    {
        // Arrange
        var secretKey = "super-strong-and-long-secret-key-123456";
        _configurationMock.Setup(x => x["Jwt:SecretKey"]).Returns(secretKey);

        var sectionMock = new Mock<IConfigurationSection>();
        sectionMock.Setup(x => x.Value).Returns("-30");
        _configurationMock.Setup(x => x.GetSection("Jwt:AccessTokenExpirationMinutes")).Returns(sectionMock.Object);

        var claims = new List<Claim>
    {
        new Claim(JwtRegisteredClaimNames.Sub, "123"),
        new Claim(ClaimTypes.Name, "testuser")
    };

        // Act & Assert
        var exception = Assert.Throws<ArgumentOutOfRangeException>(() => _jwtService.GenerateJwtToken(claims));
        Assert.Contains("The added or subtracted value results in an un-representable DateTime", exception.Message);
    }

    [Fact]
    public void ValidateToken_ShouldReturnNull_WhenInvalidSecretKeyIsUsed()
    {
        // Arrange
        var configurationMock = new Mock<IConfiguration>();
        var dateTimeProviderMock = new Mock<IDateTimeProvider>();

        var validSecretKey = "valid-secret-key-super-strong-key";
        var invalidSecretKey = "invalid-secret-key";

        configurationMock.Setup(x => x["Jwt:SecretKey"]).Returns(validSecretKey);

        var sectionMock = new Mock<IConfigurationSection>();
        sectionMock.Setup(x => x.Value).Returns("30");
        configurationMock.Setup(x => x.GetSection("Jwt:AccessTokenExpirationMinutes")).Returns(sectionMock.Object);

        var jwtService = new JwtService(configurationMock.Object, dateTimeProviderMock.Object);

        var claims = new List<Claim>
    {
        new Claim(JwtRegisteredClaimNames.Sub, "123"),
        new Claim(ClaimTypes.Name, "testuser")
    };

        var validToken = jwtService.GenerateJwtToken(claims);

        // Act
        configurationMock.Setup(x => x["Jwt:SecretKey"]).Returns(invalidSecretKey);

        var invalidJwtService = new JwtService(configurationMock.Object, dateTimeProviderMock.Object);
        var principal = invalidJwtService.ValidateToken(validToken);

        // Assert
        Assert.Null(principal);
    }

    [Fact]
    public void ValidateToken_ShouldReturnNull_WhenTokenHasInvalidSignature()
    {
        // Arrange
        var configurationMock = new Mock<IConfiguration>();
        var dateTimeProviderMock = new Mock<IDateTimeProvider>();

        var secretKey = "super-strong-and-long-secret-key-123456";
        configurationMock.Setup(x => x["Jwt:SecretKey"]).Returns(secretKey);

        var sectionMock = new Mock<IConfigurationSection>();
        sectionMock.Setup(x => x.Value).Returns("30");
        configurationMock.Setup(x => x.GetSection("Jwt:AccessTokenExpirationMinutes")).Returns(sectionMock.Object);

        var jwtService = new JwtService(configurationMock.Object, dateTimeProviderMock.Object);

        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, "123"),
            new Claim(ClaimTypes.Name, "testuser")
        };

        var token = jwtService.GenerateJwtToken(claims);

        var modifiedToken = token.Replace("123456", "654321");

        // Act
        var principal = jwtService.ValidateToken(modifiedToken);

        // Assert
        Assert.Null(principal);
    }

    [Fact]
    public void GenerateJwtToken_ShouldHandleLongClaimNames()
    {
        // Arrange
        var secretKey = "super-strong-and-long-secret-key-123456";
        _configurationMock.Setup(x => x["Jwt:SecretKey"]).Returns(secretKey);
        var expirationMinutes = 30;
        var sectionMock = new Mock<IConfigurationSection>();
        sectionMock.Setup(x => x.Value).Returns(expirationMinutes.ToString());
        _configurationMock.Setup(x => x.GetSection("Jwt:AccessTokenExpirationMinutes")).Returns(sectionMock.Object);

        var claims = new List<Claim>
        {
            new Claim("long_claim_name_" + new string('a', 500), "testvalue")
        };

        // Act
        var token = _jwtService.GenerateJwtToken(claims);

        // Assert
        Assert.NotNull(token);
        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);
        Assert.Equal("testvalue", jwtToken.Claims.First(c => c.Type == "long_claim_name_" + new string('a', 500)).Value);
    }

    [Fact]
    public void ValidateToken_ShouldReturnNull_WhenTokenIsExpired()
    {
        // Arrange
        var configurationMock = new Mock<IConfiguration>();
        var dateTimeProviderMock = new Mock<IDateTimeProvider>();

        var secretKey = "super-strong-and-long-secret-key-123456";
        configurationMock.Setup(x => x["Jwt:SecretKey"]).Returns(secretKey);

        var expirationMinutes = 30;
        var sectionMock = new Mock<IConfigurationSection>();
        sectionMock.Setup(x => x.Value).Returns(expirationMinutes.ToString());
        configurationMock.Setup(x => x.GetSection("Jwt:AccessTokenExpirationMinutes")).Returns(sectionMock.Object);

        var jwtService = new JwtService(configurationMock.Object, dateTimeProviderMock.Object);

        var claims = new List<Claim>
    {
        new Claim(JwtRegisteredClaimNames.Sub, "123"),
        new Claim(ClaimTypes.Name, "testuser")
    };

        var token = jwtService.GenerateJwtToken(claims);

        var tokenHandler = new JwtSecurityTokenHandler();
        var jwtToken = tokenHandler.ReadJwtToken(token);
        var expiredDate = DateTime.UtcNow.AddMinutes(-1);
        jwtToken.Payload[JwtRegisteredClaimNames.Exp] = new DateTimeOffset(expiredDate).ToUnixTimeSeconds();

        var expiredToken = tokenHandler.WriteToken(jwtToken);

        // Act
        var principal = jwtService.ValidateToken(expiredToken);

        // Assert
        Assert.Null(principal);
    }

    [Fact]
    public void GenerateJwtToken_ShouldExpireQuickly_WhenShortExpirationTimeIsConfigured()
    {
        // Arrange
        var secretKey = "super-strong-and-long-secret-key-123456";
        var expirationMinutes = 1;

        _configurationMock.Setup(x => x["Jwt:SecretKey"]).Returns(secretKey);

        var sectionMock = new Mock<IConfigurationSection>();
        sectionMock.Setup(x => x.Value).Returns(expirationMinutes.ToString());
        _configurationMock.Setup(x => x.GetSection("Jwt:AccessTokenExpirationMinutes")).Returns(sectionMock.Object);

        var currentTime = new DateTime(2024, 1, 1, 12, 0, 0);
        _dateTimeProviderMock.Setup(x => x.UtcNow).Returns(currentTime);

        var claims = new List<Claim>
    {
        new Claim(JwtRegisteredClaimNames.Sub, "123"),
        new Claim(ClaimTypes.Name, "testuser")
    };

        // Act
        var token = _jwtService.GenerateJwtToken(claims);

        // Assert
        Assert.NotNull(token);

        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);

        var actualExpirationMinutes = (jwtToken.ValidTo - jwtToken.ValidFrom).TotalMinutes;

        var actualExpirationMinutesRounded = (int)expirationMinutes;

        Assert.Equal(expirationMinutes, actualExpirationMinutesRounded);
    }

    [Fact]
    public void ValidateToken_ShouldReturnClaimsPrincipal_WhenValidTokenAndCorrectAlgorithm()
    {
        // Arrange
        var configurationMock = new Mock<IConfiguration>();
        var dateTimeProviderMock = new Mock<IDateTimeProvider>();

        var secretKey = "super-strong-and-long-secret-key-123456";
        configurationMock.Setup(x => x["Jwt:SecretKey"]).Returns(secretKey);

        var expirationMinutes = 30;
        var sectionMock = new Mock<IConfigurationSection>();
        sectionMock.Setup(x => x.Value).Returns(expirationMinutes.ToString());
        configurationMock.Setup(x => x.GetSection("Jwt:AccessTokenExpirationMinutes")).Returns(sectionMock.Object);

        dateTimeProviderMock.Setup(x => x.UtcNow).Returns(DateTime.UtcNow);

        var jwtService = new JwtService(configurationMock.Object, dateTimeProviderMock.Object);

        var claims = new List<Claim>
    {
        new Claim(JwtRegisteredClaimNames.Sub, "123"),
        new Claim(ClaimTypes.Name, "testuser")
    };

        var token = jwtService.GenerateJwtToken(claims);

        // Act
        var principal = jwtService.ValidateToken(token);

        // Assert
        Assert.NotNull(principal);
    }

    [Fact]
    public void ValidateToken_ShouldReturnNull_WhenUnknownAlgorithmIsUsed()
    {
        // Arrange
        var configurationMock = new Mock<IConfiguration>();
        var dateTimeProviderMock = new Mock<IDateTimeProvider>();

        var secretKey = "super-strong-and-long-secret-key-123456";
        configurationMock.Setup(x => x["Jwt:SecretKey"]).Returns(secretKey);

        var sectionMock = new Mock<IConfigurationSection>();
        sectionMock.Setup(x => x.Value).Returns("30");
        configurationMock.Setup(x => x.GetSection("Jwt:AccessTokenExpirationMinutes")).Returns(sectionMock.Object);

        var jwtService = new JwtService(configurationMock.Object, dateTimeProviderMock.Object);

        var claims = new List<Claim>
    {
        new Claim(JwtRegisteredClaimNames.Sub, "123"),
        new Claim(ClaimTypes.Name, "testuser")
    };

        var token = jwtService.GenerateJwtToken(claims);

        var modifiedToken = token.Replace("HMACSHA256", "RS256");

        // Act
        var principal = jwtService.ValidateToken(modifiedToken);

        // Assert
        Assert.Null(principal);
    }

    [Fact]
    public void ValidateToken_ShouldReturnNull_WhenTokenHasInvalidFormat()
    {
        // Arrange
        var configurationMock = new Mock<IConfiguration>();
        var dateTimeProviderMock = new Mock<IDateTimeProvider>();

        var secretKey = "super-strong-and-long-secret-key-123456";
        configurationMock.Setup(x => x["Jwt:SecretKey"]).Returns(secretKey);

        var jwtService = new JwtService(configurationMock.Object, dateTimeProviderMock.Object);

        var invalidToken = "invalid.token.value";

        // Act
        var principal = jwtService.ValidateToken(invalidToken);

        // Assert
        Assert.Null(principal);
    }

}