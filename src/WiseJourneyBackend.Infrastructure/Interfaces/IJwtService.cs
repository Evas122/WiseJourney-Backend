using System.Security.Claims;
using WiseJourneyBackend.Domain.Entities;
using WiseJourneyBackend.Domain.Enums;

namespace WiseJourneyBackend.Infrastructure.Interfaces;

public interface IJwtService
{
    string GenerateJwtToken(List<Claim> claims);
    List<Claim> GetClaims(User user, TokenType? tokenType = null);
    string GenerateEmailVerificationToken(User user);
    string GeneratePasswordResetToken(User user);
    ClaimsPrincipal? ValidateToken(string token);
}