using WiseJourneyBackend.Domain.Common;

namespace WiseJourneyBackend.Domain.Entities;

public class OpeningHour : BaseEntity
{
    public string PlaceId { get; set; } = null!;
    public bool OpenNow { get; set; }
    public ICollection<WeeklyHour> WeeklyHours { get; set; } = [];
}