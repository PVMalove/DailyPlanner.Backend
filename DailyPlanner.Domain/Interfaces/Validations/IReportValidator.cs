using DailyPlanner.Domain.Entities;
using DailyPlanner.Domain.Result;

namespace DailyPlanner.Domain.Interfaces.Validations;

public interface IReportValidator : IBaseValidator<Report>
{
    /// <summary>
    /// Валидирует создание отчета.
    /// Правило валидации:
    /// 1) Отчет должен быть создан пользователем.
    /// 2) Отчет должен иметь уникальное название.
    /// </summary>
    /// <param name="entity">Отчет.</param>
    /// <param name="user">Пользователь.</param>
    /// <returns>Объект типа <see cref="BaseResult"/>.</returns>
    BaseResult ValidateCreating(Report entity, User user);
}