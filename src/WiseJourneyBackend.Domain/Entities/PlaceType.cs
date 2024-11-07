using WiseJourneyBackend.Domain.Common;

namespace WiseJourneyBackend.Domain.Entities;

public class PlaceType : BaseEntity
{
    public Guid PlaceId { get; set; }
    public string TypeName { get; set; } = null!;
}