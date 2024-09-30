using Asp.Versioning;
using DailyPlanner.API.Filters.ReportControllersFilter;
using DailyPlanner.Domain.DTO.Report;
using DailyPlanner.Domain.Interfaces.Services;
using DailyPlanner.Domain.Result;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DailyPlanner.API.Controllers;

/// <summary>
/// Report controller
/// </summary>
/// <response code="200">If report was created/deleted/updated/received</response>
/// <response code="400">If report was not created/deleted/updated/received</response>
/// <response code="500">If internal server error occured</response>
/// <response code="401">If user is unauthorized</response>
/// <response code="403">If user is forbidden to make request</response>
[Authorize]
[ApiVersion("1.0")]
[ApiVersion("2.0")]
[ProducesResponseType(StatusCodes.Status200OK)]
[ProducesResponseType(StatusCodes.Status400BadRequest)]
[ProducesResponseType(StatusCodes.Status500InternalServerError)]
[ProducesResponseType(StatusCodes.Status401Unauthorized)]
public class ReportController(IReportService reportService) : ApplicationController
{
    /// <summary>
    /// Получает отчет по идентификатору [id]
    /// </summary>
    /// <remarks>
    /// Пример запроса:
    /// 
    ///     GET
    ///     {
    ///         "id": 1
    ///     }
    /// 
    /// </remarks>
    /// <param name="id">Repor Id (long)</param>
    [HttpGet("{id}")]
    [ReportOwnershipValidationFilter("id")]
    public async Task<ActionResult<BaseResult<ReportDto>>> GetById(long id)
    {
        var response = await reportService.GetReportByIdAsync(id);
        if (response.IsSuccess)
        {
            return Ok(response);
        }
        return BadRequest(response);
    }
    
    /// <summary>
    /// Получает отчеты пользователя по идентификатору [userId]
    /// </summary>
    /// <remarks>
    /// Пример запроса:
    ///
    ///     GET
    ///     {
    ///         "userId": 1 
    ///     }
    /// 
    /// </remarks>
    /// <param name="userId">User Id (long)</param>
    [HttpGet("{userId:long:min(1)}/all")]
    [UserIdValidationFilter("userId")]
    public async Task<ActionResult<BaseResult<List<ReportDto>>>> GetByUserId(long userId)
    {
        var response = await reportService.GetReportsAsync(userId);
        if (response.IsSuccess)
        {
            return Ok(response);
        }
        return BadRequest(response);
    }
    
    /// <summary>
    /// Создает отчет
    /// </summary>
    /// <remarks>
    /// Пример запроса:
    /// 
    ///     POST
    ///     { 
    ///        "title": "title",
    ///        "description": "description"
    ///        "userId": 1,
    ///     }
    /// 
    /// </remarks>
    /// <param name="reportDto"></param>
    /// <returns></returns>
    [HttpPost("create")]
    [UserIdValidationFilter("userId")]
    public async Task<ActionResult<BaseResult<ReportDto>>> Create([FromBody] CreateReportDto reportDto)
    {
        var response = await reportService.CreateReportAsync(reportDto);
        if (response.IsSuccess)
        {
            return Ok(response);
        }
        return BadRequest(response);
    }
    
    /// <summary>
    /// Обновляет отчет
    /// </summary>
    /// <param name="reportDto"></param>
    /// <returns></returns>
    [HttpPut("update")]
    [ReportOwnershipValidationFilter("id")]
    public async Task<ActionResult<BaseResult<ReportDto>>> Update([FromBody] UpdateReportDto reportDto)
    {
        var response = await reportService.UpdateReportAsync(reportDto);
        if (response.IsSuccess)
        {
            return Ok(response);
        }
        return BadRequest(response);
    }

    /// <summary>
    /// Удаляет отчет
    /// </summary>
    /// <remarks>
    /// Пример запроса:
    /// 
    ///     DELETE
    ///     {
    ///         "id": 1 
    ///     }
    /// 
    /// </remarks>
    /// <param name="id">Repor Id (long)</param>
    /// <response code="200">Success</response>
    /// <response code="400">If the request is not valid</response>
    /// <returns></returns>
    [HttpDelete("{id:long}/delete")]
    [ReportOwnershipValidationFilter("id")]
    public async Task<ActionResult<BaseResult<ReportDto>>> DeleteById(long id)
    {
        var response = await reportService.DeleteReportAsync(id);
        if (response.IsSuccess)
        {
            return Ok(response);
        }
        return BadRequest(response);
    }
}