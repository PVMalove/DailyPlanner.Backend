using DailyPlanner.Domain.DTO.User;
using DailyPlanner.Domain.Result;

namespace DailyPlanner.Domain.Interfaces.Services;

/// <summary>
/// Интерфейс для управления аутентификацией и авторизацией в приложении.
/// </summary>
public interface IAuthService
{
    /// <summary>
    /// Регистрация нового пользователя.
    /// </summary>
    /// <param name="registerUserDto">Данные для регистрации.</param>
    /// <returns>Результат регистрации с информацией о пользователе.</returns>
    Task<BaseResult<UserDto>> Register(RegisterUserDto registerUserDto);

    /// <summary>
    /// Вход существующего пользователя.
    /// </summary>
    /// <param name="loginUserDto">Учетные данные для входа.</param>
    /// <returns>Токен аутентификации после успешного входа.</returns>
    Task<BaseResult<TokenDto>> Login(LoginUserDto loginUserDto);
}