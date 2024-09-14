namespace DailyPlanner.Domain.DTO.User;

public record RegisterUserDto
{
    public string Login { get; init; }
    public string Password { get; init; }
    public string PasswordConfirm { get; init; }
}