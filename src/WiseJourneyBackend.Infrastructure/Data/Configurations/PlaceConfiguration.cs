using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using WiseJourneyBackend.Domain.Entities;

namespace WiseJourneyBackend.Infrastructure.Data.Configurations;

internal sealed class PlaceConfiguration : IEntityTypeConfiguration<Place>
{
    public void Configure(EntityTypeBuilder<Place> builder)
    {
        builder.HasKey(p => p.Id);

        builder.HasOne(p => p.Geometry)
            .WithOne()
            .HasForeignKey<Geometry>(g => g.PlaceId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(p => p.OpeningHour)
            .WithOne()
            .HasForeignKey<OpeningHour>(oh => oh.PlaceId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(p => p.PlaceTypes)
            .WithOne()
            .HasForeignKey(pt => pt.PlaceId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}