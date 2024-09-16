using DailyPlanner.Domain.Interfaces;

namespace DailyPlanner.Domain.Entities;

public class User : IEntityId<long>
{
    public long Id { get; set; }
    public string Login { get; init; }
    public string Password { get; init; }
    public List<Report> Reports { get; init; }
}