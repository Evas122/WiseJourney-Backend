using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WiseJourneyBackend.Domain.Entities.Auth;

namespace WiseJourneyBackend.Infrastructure.Data.Configurations.Auth;

internal sealed class UserConfigiration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(u => u.Id);

        builder.HasMany(u => u.RefreshTokens)
              .WithOne(r => r.User)
              .HasForeignKey(r => r.UserId)
              .IsRequired();

        builder.HasMany(u => u.Trips)
              .WithOne(t => t.User)
              .HasForeignKey(t => t.UserId)
              .IsRequired();
    }
}