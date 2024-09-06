using DailyPlanner.Domain.Interfaces;

namespace DailyPlanner.Domain.Entities;

public class User : IEntityId<long>
{
    public long Id { get; set; }
    public string Login { get; set; }
    public string Password { get; set; }
    public List<Report> Reports { get; set; }
}