using DailyPlanner.Domain.Interfaces;

namespace DailyPlanner.Domain.Entities;

public class UserToken : IEntityId<long>
{
    public long Id { get; set; }
    public string RefreshToken { get; set; }
    public DateTime RefreshTokenExpireTime { get; set; }
    public User User { get; init; }
    public long UserId { get; init; }
}