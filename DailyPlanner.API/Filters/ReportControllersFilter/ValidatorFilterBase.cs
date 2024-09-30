namespace DailyPlanner.API.Filters.ReportControllersFilter;

/// <summary>
/// Базовый класс для валидаторов в контроллере отчетов
/// </summary>
public abstract class ValidatorFilterBase : Attribute
{
    /// <summary>
    /// Схема аутентификации по умолчанию
    /// </summary>
    protected const string AuthenticationScheme = "Bearer";

    /// <summary>
    /// Имя идентификатора, который нужно получить из маршрута
    /// </summary>
    protected string IdentifierName { get; init; }
}