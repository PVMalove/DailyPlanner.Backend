using DailyPlanner.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DailyPlanner.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.Property(user => user.Id).ValueGeneratedOnAdd();
        builder.Property(user => user.Login).IsRequired().HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH);
        builder.Property(user => user.Password).IsRequired();
        builder.HasMany<Report>(user => user.Reports)
            .WithOne(report => report.User)
            .HasForeignKey(report => report.UserId)
            .HasPrincipalKey(user => user.Id);
        builder.HasMany(u => u.Roles)
            .WithMany(r => r.Users)
            .UsingEntity<UserRole>(e => e.HasOne<Role>().WithMany().HasForeignKey(ur => ur.RoleId),
                e => e.HasOne<User>().WithMany().HasForeignKey(ur => ur.UserId));
    }
}