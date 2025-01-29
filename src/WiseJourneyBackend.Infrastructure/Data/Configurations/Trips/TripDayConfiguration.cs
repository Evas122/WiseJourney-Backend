using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using WiseJourneyBackend.Domain.Entities.Trips;

namespace WiseJourneyBackend.Infrastructure.Data.Configurations.Trips;

internal sealed class TripDayConfiguration : IEntityTypeConfiguration<TripDay>
{
    public void Configure(EntityTypeBuilder<TripDay> builder)
    {
        builder.HasKey(td => td.Id);

        builder.HasMany(td => td.TripPlaces)
              .WithOne(tp => tp.TripDay)
              .HasForeignKey(tp => tp.TripDayId)
              .IsRequired();

        builder.HasOne(td => td.Trip)
              .WithMany(t => t.TripDays)
              .HasForeignKey(td => td.TripId)
              .IsRequired();
    }
}