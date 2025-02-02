﻿using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using WiseJourneyBackend.Domain.Entities.Places;

namespace WiseJourneyBackend.Infrastructure.Data.Configurations.Places;

internal sealed class PlaceTypeConfiguration : IEntityTypeConfiguration<PlaceType>
{
    public void Configure(EntityTypeBuilder<PlaceType> builder)
    {
        builder.HasKey(pt => pt.Id);

        builder.HasOne<Place>()
            .WithMany(p => p.PlaceTypes)
            .HasForeignKey(pt => pt.PlaceId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}