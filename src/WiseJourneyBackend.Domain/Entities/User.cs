using WiseJourneyBackend.Domain.Common;

namespace WiseJourneyBackend.Domain.Entities;
public class User : BaseEntity
{
    public string UserName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public bool EmailConfirmed { get; set; }
    public string PasswordHash { get; set; } = null!;
}