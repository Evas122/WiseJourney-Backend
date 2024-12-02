using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;
using System.Security.Cryptography;
using WiseJourneyBackend.Application.Dtos.Auth;
using WiseJourneyBackend.Application.Extensions;
using WiseJourneyBackend.Application.Interfaces;
using WiseJourneyBackend.Domain.Entities;
using WiseJourneyBackend.Domain.Entities.Auth;
using WiseJourneyBackend.Domain.Enums;
using WiseJourneyBackend.Domain.Exceptions;
using WiseJourneyBackend.Domain.Repositories;
using WiseJourneyBackend.Infrastructure.Interfaces;

namespace WiseJourneyBackend.Infrastructure.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IJwtService _jwtService;
    private readonly IPasswordHasher<User> _passwordHasher;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IConfiguration _configuration;
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly IEmailService _emailService;

    public AuthService(IUserRepository userRepository,
        IRefreshTokenRepository refreshTokenRepository,
        IJwtService jwtService,
        IPasswordHasher<User> passwordHasher,
        IDateTimeProvider dateTimeProvider,
        IConfiguration configuration,
        IHttpContextAccessor contextAccessor,
        IEmailService emailService)
    {
        _userRepository = userRepository;
        _refreshTokenRepository = refreshTokenRepository;
        _jwtService = jwtService;
        _passwordHasher = passwordHasher;
        _dateTimeProvider = dateTimeProvider;
        _configuration = configuration;
        _contextAccessor = contextAccessor;
        _emailService = emailService;
    }

    public async Task RegisterAsync(RegisterDto registerDto)
    {
        bool emailExists = await _userRepository.IsEmailExistsAsync(registerDto.Email);
        if (emailExists)
        {
            throw new AlreadyExistsException("This email already in use", registerDto.Email);
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

        var emailVerificationToken = _jwtService.GenerateEmailVerificationToken(user);

        await _emailService.SendVerificationEmail(user, emailVerificationToken);

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

        return new AuthResultDto(accessToken, refreshToken);
    }

    public async Task<AuthResultDto> RefreshTokenAsync(string refreshToken)
    {
        var refreshTokenEntity = await _refreshTokenRepository.GetRefreshTokenByTokenAsync(refreshToken);

        if (refreshTokenEntity == null || refreshTokenEntity.ExpiresAtUtc <= _dateTimeProvider.UtcNow || refreshTokenEntity.IsRevoked)
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

        return new AuthResultDto(newJwtToken, refreshToken);
    }

    public async Task Logout()
    {
        var userId = _contextAccessor.GetUserId();
        var refreshTokens = await _refreshTokenRepository.GetAllActiveTokensAsync(userId);
        //TODO clear cookies instead tokens or something else
        await _refreshTokenRepository.RemoveUserRefreshTokensAsync(refreshTokens);
    }

    private async Task<string> RetrieveRefreshTokenAsync(Guid userId)
    {
        var existingRefreshToken = await _refreshTokenRepository.GetRefreshTokenByUserIdAsync(userId);

        if (existingRefreshToken != null && existingRefreshToken.IsRevoked != true && existingRefreshToken.ExpiresAtUtc !> _dateTimeProvider.UtcNow)
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

    public async Task SendPasswordResetAsync(string email)
    {
        var user = await _userRepository.GetByEmailAsync(email);
        if (user == null)
        {
            throw new NotFoundException(nameof(user), email);
        }

        var resetPasswordToken = _jwtService.GeneratePasswordResetToken(user);

        await _emailService.SendResetPasswordEmail(user, resetPasswordToken);

    }

    public async Task<bool> VerifyEmailAsync(string token)
    {
        var principal = _jwtService.ValidateToken(token);
        if (principal == null)
            return false;

        var tokenTypeClaim = principal.FindFirst("tokenType")?.Value;
        if (tokenTypeClaim != TokenType.EmailConfirmation.ToString())
            return false;

        var userEmail = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

        if (string.IsNullOrEmpty(userEmail))
        {
            throw new NotFoundException("User", "claim not found in token.");
        }

        var user = await _userRepository.GetByEmailAsync(userEmail);

        if (user == null)
        {
            throw new NotFoundException(nameof(user), userEmail);  
        }

        user.EmailConfirmed = true;
        await _userRepository.UpdateAsync(user);
        return true;
    }

    public async Task<bool> ResetPasswordAsync(string token, string newPassword)
    {
        var principal = _jwtService.ValidateToken(token);
        if (principal == null)
            return false;

        var tokenTypeClaim = principal.FindFirst("tokenType")?.Value;
        if (tokenTypeClaim != TokenType.PasswordReset.ToString())
            return false;

        var userEmail = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

        if (string.IsNullOrEmpty(userEmail))
        {
            throw new NotFoundException("User", "claim not found in token.");
        }

        var user = await _userRepository.GetByEmailAsync(userEmail);

        if (user == null)
        {
            throw new NotFoundException(nameof(user), userEmail);
        }

        user.PasswordHash = _passwordHasher.HashPassword(user, newPassword);
        await _userRepository.UpdateAsync(user);
        return true;
    }
}