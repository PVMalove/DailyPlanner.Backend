using DailyPlanner.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DailyPlanner.Persistence.Configurations;

public class ReportConfiguration : IEntityTypeConfiguration<Report>
{
    public void Configure(EntityTypeBuilder<Report> builder)
    {
        builder.Property(report => report.Id).IsRequired();
        builder.Property(report => report.Name).IsRequired().HasMaxLength(100);
        builder.Property(report => report.Description).IsRequired().HasMaxLength(2000);
    }
}