using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WiseJourneyBackend.Domain.Entities;

namespace WiseJourneyBackend.Infrastructure.Data.Configurations;

internal sealed class UserConfigiration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(u  => u.Id);

        builder.HasMany(u => u.RefreshTokens)
              .WithOne(r => r.User)
              .HasForeignKey(r => r.UserId)
              .IsRequired();
    }
}