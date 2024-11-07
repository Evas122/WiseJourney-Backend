using WiseJourneyBackend.Domain.Common;

namespace WiseJourneyBackend.Domain.Entities;

public class Geometry : BaseEntity
{
    public Guid PlaceId { get; set; }
    public float Latitude { get; set; }
    public float Longitude { get; set; }
}