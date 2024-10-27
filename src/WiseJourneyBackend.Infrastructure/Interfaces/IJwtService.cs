using System.Security.Claims;
using WiseJourneyBackend.Domain.Entities;

namespace WiseJourneyBackend.Infrastructure.Interfaces;

public interface IJwtService
{
    string GenerateJwtToken(List<Claim> claims);
    List<Claim> GetClaims(User user);
}