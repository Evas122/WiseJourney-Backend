using WiseJourneyBackend.Domain.Common;

namespace WiseJourneyBackend.Domain.Entities.Places;

public class Geometry : BaseEntity
{
    public string PlaceId { get; set; } = null!;
    public double Latitude { get; set; }
    public double Longitude { get; set; }
}