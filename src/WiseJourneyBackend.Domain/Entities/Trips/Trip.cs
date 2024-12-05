using WiseJourneyBackend.Domain.Common;
using WiseJourneyBackend.Domain.Entities.Auth;

namespace WiseJourneyBackend.Domain.Entities.Trips;

public class Trip : BaseEntity
{
    public string Name { get; set; } = null!;
    public DateTime StartDateUtc { get; set; }
    public DateTime EndDateUtc { get; set; }
    public ICollection<TripDay> TripDays { get; set; } = [];
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
}