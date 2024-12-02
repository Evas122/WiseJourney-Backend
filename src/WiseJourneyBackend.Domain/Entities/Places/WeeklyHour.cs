using WiseJourneyBackend.Domain.Common;
using WiseJourneyBackend.Domain.Enums;

namespace WiseJourneyBackend.Domain.Entities.Places;

public class WeeklyHour : BaseEntity
{
    public Day Day { get; set; }
    public DateTime OpenTime { get; set; }
    public DateTime CloseTime { get; set; }
    public Guid OpeningHourId { get; set; }
    public OpeningHour OpeningHour { get; set; } = null!;
}