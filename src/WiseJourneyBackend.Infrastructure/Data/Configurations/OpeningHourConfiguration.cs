using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using WiseJourneyBackend.Domain.Entities;

namespace WiseJourneyBackend.Infrastructure.Data.Configurations;

internal sealed class OpeningHourConfiguration : IEntityTypeConfiguration<OpeningHour>
{
    public void Configure(EntityTypeBuilder<OpeningHour> builder)
    {
        builder.HasKey(oh => oh.Id);

        builder.HasOne<Place>()
            .WithOne()
            .HasForeignKey<OpeningHour>(oh => oh.PlaceId) 
            .OnDelete(DeleteBehavior.Cascade); 

        builder.HasMany(oh => oh.WeeklyHours) 
            .WithOne() 
            .HasForeignKey(wh => wh.OpeningHourId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}