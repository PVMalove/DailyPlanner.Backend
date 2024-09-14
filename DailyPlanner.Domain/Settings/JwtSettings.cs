namespace DailyPlanner.Domain.Settings;

public class JwtSettings
{
    public const string DefaultSection = "JwtSettings";
    
    public string Issuer { get; init; }
    public string Audience { get; init; }
    public string Authority { get; init; }
    public string JwtKey { get; init; }
    public int LifeTime { get; init; }
    public int RefreshTokenValidityInDays { get; init; }
}