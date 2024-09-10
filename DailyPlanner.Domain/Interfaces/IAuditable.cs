namespace DailyPlanner.Domain.Interfaces;

public interface IAuditable
{
    DateTime CreatedAt { get; set; }
    public long CreatedBy { get; set; }
    DateTime? UpdatedAt { get; set; }
    public long? UpdatedBy { get; set; }
}