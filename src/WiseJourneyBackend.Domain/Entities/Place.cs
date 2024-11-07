using WiseJourneyBackend.Domain.Common;

namespace WiseJourneyBackend.Domain.Entities;

public class Place : BaseEntity
{
    public string Name { get; set; } = null!;
    public string FullAddress { get; set; } = null!;
    public string ShortAddress { get; set; } = null!;
    public float Rating { get; set; }
    public int UserRatingsTotal { get; set; }
    public int PriceLevel { get; set; }
    public Geometry Geometry { get; set; } = null!;
    public OpeningHour OpeningHour { get; set; } = null!;
    public ICollection<PlaceType> PlaceTypes { get; set; } = [];
}