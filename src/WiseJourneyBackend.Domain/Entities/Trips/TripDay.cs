using WiseJourneyBackend.Domain.Common;

namespace WiseJourneyBackend.Domain.Entities.Trips;

public class TripDay : BaseEntity
{
    public int Day { get; set; }
    public Guid TripId { get; set; }
    public Trip Trip { get; set; } = null!;
    public ICollection<TripPlace> TripPlaces { get; set; } = [];
}