using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using WiseJourneyBackend.Domain.Entities.Trips;

namespace WiseJourneyBackend.Infrastructure.Data.Configurations.Trips;

internal sealed class TripPlaceConfiguration : IEntityTypeConfiguration<TripPlace>
{
    public void Configure(EntityTypeBuilder<TripPlace> builder)
    {
        builder.HasKey(tp => tp.Id);

        builder.HasOne(tp => tp.Place)
               .WithMany()
               .HasForeignKey(tp => tp.PlaceId)
               .IsRequired();

        builder.HasOne(tp => tp.TripDay)
               .WithMany(td => td.TripPlaces)
               .HasForeignKey(tp => tp.TripDayId)
               .IsRequired();
    }
}