using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WiseJourneyBackend.Domain.Entities.Trips;

namespace WiseJourneyBackend.Infrastructure.Data.Configurations.Trips;

internal sealed class TripConfiguration : IEntityTypeConfiguration<Trip>
{
    public void Configure(EntityTypeBuilder<Trip> builder)
    {
        builder.HasKey(t => t.Id);

        builder.HasMany(t => t.TripDays)
              .WithOne(td => td.Trip)
              .HasForeignKey(td => td.TripId)
              .IsRequired();

        builder.HasOne(t => t.User)
              .WithMany(u => u.Trips)
              .HasForeignKey(t => t.UserId)
              .IsRequired();
    }
}