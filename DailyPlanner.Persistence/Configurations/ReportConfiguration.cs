using DailyPlanner.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DailyPlanner.Persistence.Configurations;

public class ReportConfiguration : IEntityTypeConfiguration<Report>
{
    public void Configure(EntityTypeBuilder<Report> builder)
    {
        builder.Property(report => report.Id).IsRequired();
        builder.Property(report => report.Name).IsRequired().HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH);
        builder.Property(report => report.Description).IsRequired().HasMaxLength(Constants.MAX_HIGH_TEXT_LENGTH);
    }
}