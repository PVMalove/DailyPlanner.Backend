using DailyPlanner.Domain.DTO.Report;
using DailyPlanner.Domain.DTO.Role;
using DailyPlanner.Domain.Result;

namespace DailyPlanner.Domain.Interfaces.Services;

/// <summary>
/// Интерфейс для сервиса отчетов.
/// </summary>
public interface IReportService
{
    /// <summary>
    /// Асинхронно получает отчеты для указанного пользователя по его идентификатору.
    /// </summary>
    /// <param name="userId">Идентификатор пользователя.</param>
    /// <returns>Объект типа <see cref="CollectionResult{ReportDto}"/>, содержащий коллекцию отчетов.</returns>
    Task<CollectionResult<ReportDto>> GetReportsAsync(long userId);
    
    /// <summary>
    /// Асинхронно получает отчет по его идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор отчета.</param>
    /// <returns>Объект типа <see cref="BaseResult{ReportDto}"/>, содержащий отчет.</returns>
    Task<BaseResult<ReportDto>> GetReportByIdAsync(long id);
    
    /// <summary>
    /// Асинхронно создает отчет с базовыми параметрами.
    /// </summary>
    /// <param name="reportDto">Объект типа с базовыми параметрами отчета <see cref="CreateReportDto"/>.</param>
    /// <returns>Объект типа <see cref="BaseResult{ReportDto}"/>.</returns>
    Task<BaseResult<ReportDto>> CreateReportAsync(CreateReportDto reportDto);
    
    /// <summary>
    /// Асинхронно обновляет отчет с базовыми параметрами.
    /// </summary>
    /// <param name="reportDto">Объект типа с базовыми параметрами отчета <see cref="UpdateReportDto"/>.</param>
    /// <returns>Объект типа <see cref="BaseResult{ReportDto}"/>.</returns>
    Task<BaseResult<ReportDto>> UpdateReportAsync(UpdateReportDto reportDto);
    
    /// <summary>
    /// Асинхронно удаление отчета по его идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор отчета.</param>
    /// <returns>Объект типа <see cref="BaseResult{ReportDto}"/>, содержащий отчет.</returns>
    Task<BaseResult<ReportDto>> DeleteReportAsync(long id);
}
