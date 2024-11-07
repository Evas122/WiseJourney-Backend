using WiseJourneyBackend.Application.Dtos.Auth;
using WiseJourneyBackend.Domain.Entities;

namespace WiseJourneyBackend.Application.Interfaces;

public interface IAuthService
{
    Task<AuthResultDto> LoginAsync(LoginDto loginDto);
    Task Logout();
    Task<AuthResultDto> RefreshTokenAsync(string refreshToken);
    Task RegisterAsync(RegisterDto registerDto);
    Task SendPasswordResetAsync(string email);
    Task<bool> VerifyEmailAsync(string token);
    Task<bool> ResetPasswordAsync(string token, string newPassword);
}