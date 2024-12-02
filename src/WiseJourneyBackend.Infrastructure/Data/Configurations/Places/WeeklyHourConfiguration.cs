using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using WiseJourneyBackend.Domain.Entities.Places;

namespace WiseJourneyBackend.Infrastructure.Data.Configurations.Places;

internal sealed class WeeklyHourConfiguration : IEntityTypeConfiguration<WeeklyHour>
{
    public void Configure(EntityTypeBuilder<WeeklyHour> builder)
    {
        builder.HasKey(wh => wh.Id);

        builder.HasOne(wh => wh.OpeningHour)
            .WithMany(oh => oh.WeeklyHours)
            .HasForeignKey(wh => wh.OpeningHourId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}