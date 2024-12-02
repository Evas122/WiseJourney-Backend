using WiseJourneyBackend.Domain.Common;
using WiseJourneyBackend.Domain.Entities.Places;

namespace WiseJourneyBackend.Domain.Entities.Trips;

public class TripPlace : BaseEntity
{
    public string PlaceId { get; set; } = null!;
    public Place Place { get; set; } = null!;
    public Guid TripDayId { get; set; }
    public TripDay TripDay { get; set; } = null!;
    public DateTime? ScheduleTimeUtc { get; set; }
}