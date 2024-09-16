using DailyPlanner.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DailyPlanner.Persistence.Configurations;

public class UserTokenConfiguration : IEntityTypeConfiguration<UserToken>
{
    public void Configure(EntityTypeBuilder<UserToken> builder)
    {
        builder.Property(t => t.Id).ValueGeneratedOnAdd();
        builder.Property(t => t.RefreshToken).IsRequired();
        builder.Property(t => t.RefreshTokenExpiryTime).IsRequired();
    }
}