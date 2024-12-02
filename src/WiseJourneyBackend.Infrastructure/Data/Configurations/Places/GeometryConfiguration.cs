using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WiseJourneyBackend.Domain.Entities.Places;

namespace WiseJourneyBackend.Infrastructure.Data.Configurations.Places;

internal sealed class GeometryConfiguration : IEntityTypeConfiguration<Geometry>
{
    public void Configure(EntityTypeBuilder<Geometry> builder)
    {
        builder.HasKey(g => g.Id);

        builder.HasOne<Place>()
            .WithOne()
            .HasForeignKey<Geometry>(g => g.PlaceId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}