using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System.Security.Cryptography;
using WiseJourneyBackend.Application.Dtos.Auth;
using WiseJourneyBackend.Application.Extensions;
using WiseJourneyBackend.Application.Interfaces;
using WiseJourneyBackend.Domain.Entities;
using WiseJourneyBackend.Domain.Exceptions;
using WiseJourneyBackend.Domain.Repositories;
using WiseJourneyBackend.Infrastructure.Interfaces;

namespace WiseJourneyBackend.Infrastructure.Services;

public class AuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IJwtService _jwtService;
    private readonly IPasswordHasher<User> _passwordHasher;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IConfiguration _configuration;
    private readonly IHttpContextAccessor _httpContenxtAccessor;

    public AuthService(IUserRepository userRepository, IRefreshTokenRepository refreshTokenRepository, IJwtService jwtService, IPasswordHasher<User> passwordHasher, IDateTimeProvider dateTimeProvider, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
    {
        _userRepository = userRepository;
        _refreshTokenRepository = refreshTokenRepository;
        _jwtService = jwtService;
        _passwordHasher = passwordHasher;
        _dateTimeProvider = dateTimeProvider;
        _configuration = configuration;
        _httpContenxtAccessor = httpContextAccessor;
    }

    public async Task RegisterAsync(RegisterDto registerDto)
    {
        bool emailExists = await _userRepository.IsEmailExistsAsync(registerDto.Email);
        if (emailExists)
        {
            throw new AlreadyExistsException("This email already exists", registerDto.Email);
        }

        var user = new User
        {
            Email = registerDto.Email,
            UserName = registerDto.UserName,
            CreatedAtUtc = _dateTimeProvider.UtcNow,
            UpdatedAtUtc = _dateTimeProvider.UtcNow
        };

        user.PasswordHash = _passwordHasher.HashPassword(user, registerDto.Password);

        await _userRepository.AddAsync(user);
    }

    public async Task<AuthResultDto> LoginAsync(LoginDto loginDto)
    {
        var user = await _userRepository.GetByEmailAsync(loginDto.Email);
        if (user == null)
        {
            throw new NotFoundException(nameof(user), loginDto.Email);
        }

        var passwordVerificationResult = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, loginDto.Password);
        if (passwordVerificationResult == PasswordVerificationResult.Failed)
        {
            throw new BadRequestException("Email or password is incorrect");
        }

        var claims = _jwtService.GetClaims(user);
        var accessToken = _jwtService.GenerateJwtToken(claims);
        var refreshToken = await RetrieveRefreshTokenAsync(user.Id);

        return new AuthResultDto
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken
        };
    }

    public async Task<AuthResultDto> RefreshTokenAsync(string refreshToken)
    {
        var refreshTokenEntity = await _refreshTokenRepository.GetRefreshTokenByTokenAsync(refreshToken);

        if (refreshTokenEntity == null)
        {
            throw new BadRequestException("Invalid refresh token");
        }

        var userId = refreshTokenEntity.UserId;
        var user = await _userRepository.GetByIdAsync(userId);

        if (user == null)
        {
            throw new NotFoundException(nameof(user), userId.ToString());
        }

        var claims = _jwtService.GetClaims(user);
        var newJwtToken = _jwtService.GenerateJwtToken(claims);

        return new AuthResultDto
        {
            AccessToken = newJwtToken,
            RefreshToken = refreshToken
        };
    }

    public async Task Logout()
    {
        var userId = _httpContenxtAccessor.GetUserId();
        var refreshTokens = await _refreshTokenRepository.GetAllActiveTokensAsync(userId);
        foreach (var refreshToken in refreshTokens)
        {
            refreshToken.IsRevoked = true;
            refreshToken.RevokedAtUtc = _dateTimeProvider.UtcNow;
        }
        await _refreshTokenRepository.UpdateUserRefreshTokens(refreshTokens);
    }

    private async Task<string> RetrieveRefreshTokenAsync(Guid userId)
    {
        var existingRefreshToken = await _refreshTokenRepository.GetRefreshTokenByUserIdAsync(userId);

        if (existingRefreshToken != null)
        {
            return existingRefreshToken.Token;
        }

        var newRefreshToken = new RefreshToken
        {
            Id = Guid.NewGuid(),
            Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
            UserId = userId,
            ExpiresAtUtc = _dateTimeProvider.UtcNow.AddDays(_configuration.GetValue<int>("Jwt:RefreshTokenExpirationDays")),
            CreatedAtUtc = _dateTimeProvider.UtcNow
        };
        await _refreshTokenRepository.AddAsync(newRefreshToken);

        return newRefreshToken.Token;
    }
}