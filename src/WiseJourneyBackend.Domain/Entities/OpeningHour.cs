using WiseJourneyBackend.Domain.Common;

namespace WiseJourneyBackend.Domain.Entities;

public class OpeningHour : BaseEntity
{
    public Guid PlaceId { get; set; }
    public bool OpenNow { get; set; }
    public ICollection<WeeklyHour> WeeklyHours { get; set; } = [];
}