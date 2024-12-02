using WiseJourneyBackend.Domain.Common;

namespace WiseJourneyBackend.Domain.Entities.Places;

public class PlaceType : BaseEntity
{
    public string PlaceId { get; set; } = null!;
    public string TypeName { get; set; } = null!;
}