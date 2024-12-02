using WiseJourneyBackend.Domain.Common;
using WiseJourneyBackend.Domain.Entities.Trips;

namespace WiseJourneyBackend.Domain.Entities.Auth;

public class User : BaseEntity
{
    public string UserName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public bool EmailConfirmed { get; set; } = false;
    public string PasswordHash { get; set; } = null!;
    public ICollection<RefreshToken> RefreshTokens { get; set; } = [];
    public ICollection<Trip> Trips { get; set; } = [];
}