using DailyPlanner.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DailyPlanner.Persistence.Configurations;

public class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.Property(r => r.Id).ValueGeneratedOnAdd();
        builder.Property(r => r.Name).IsRequired().HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH);
    }
}