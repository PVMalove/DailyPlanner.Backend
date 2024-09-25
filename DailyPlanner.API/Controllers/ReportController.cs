using Asp.Versioning;
using DailyPlanner.Domain.DTO.Report;
using DailyPlanner.Domain.Interfaces.Services;
using DailyPlanner.Domain.Result;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DailyPlanner.API.Controllers;

[Authorize]
[ApiVersion("1.0")]
[ApiVersion("2.0")]
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
    /// <response code="200">Success</response>
    /// <response code="401">If the user is unauthorized</response>
    /// <response code="400">If the request is not valid</response>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
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
    /// <response code="200">Success</response>
    /// <response code="401">If the user is unauthorized</response>
    /// <response code="400">If the request is not valid</response>
    [HttpGet("{userId:long}/all")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
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
    ///        "userId": 1,
    ///        "title": "title",
    ///        "description": "description"
    ///     }
    /// 
    /// </remarks>
    /// <param name="reportDto"></param>
    /// <returns></returns>
    [HttpPost("create")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
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
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
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
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
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