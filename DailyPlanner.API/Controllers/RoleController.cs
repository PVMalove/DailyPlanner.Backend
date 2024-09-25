using System.Net.Mime;
using Asp.Versioning;
using DailyPlanner.Domain.DTO.Role;
using DailyPlanner.Domain.Entities;
using DailyPlanner.Domain.Interfaces.Services;
using DailyPlanner.Domain.Result;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DailyPlanner.API.Controllers;


[ApiVersion("2.0")]
[Authorize(Roles = "Admin")]
[Consumes(MediaTypeNames.Application.Json)]
public class RoleController(IRoleService roleService) : ApplicationController
{
    /// <summary>
    /// Создает роль
    /// </summary>
    /// <remarks>
    /// Пример запроса:
    /// 
    ///     POST
    ///     {
    ///        "name": "Anna"
    ///     }
    /// 
    /// </remarks>
    /// <response code="200">Success</response>
    /// <response code="401">If the user is unauthorized</response>
    /// <response code="400">If the request is not valid</response>
    [HttpPost("create")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<BaseResult<Role>>> GetById([FromBody] CreateRoleDto roleDto)
    {
        var response = await roleService.CreateAsync(roleDto);
        if (response.IsSuccess)
        {
            return Ok(response);
        }
        return BadRequest(response);
    }
    
    
    /// <summary>
    /// Обновляет роль
    ///
    /// </summary>
    /// <remarks>
    /// Пример запроса:
    ///
    ///     PUT
    ///
    ///     {
    ///        "id": 1,
    ///        "name": "Anna"
    ///     }
    ///
    /// </remarks>
    /// <response code="200">Success</response>
    /// <response code="401">If the user is unauthorized</response>
    /// <response code="400">If the request is not valid</response>
    [HttpPut("update")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<BaseResult<Role>>> Update([FromBody] RoleDto roleDto)
    {
        var response = await roleService.UpdateAsync(roleDto);
        if (response.IsSuccess)
        {
            return Ok(response);
        }
        return BadRequest(response);
    }
    
    [HttpDelete("{id:long}/delete")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<BaseResult<Role>>> Delete(long id)
    {
        var response = await roleService.DeleteAsync(id);
        if (response.IsSuccess)
        {
            return Ok(response);
        }
        return BadRequest(response);
    }
    
    [HttpPost("add-role-for-user")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<BaseResult<Role>>> AddRoleForUser([FromBody] UserRoleDto roleDto)
    {
        var response = await roleService.AddRoleForUserAsync(roleDto);
        if (response.IsSuccess)
        {
            return Ok(response);
        }
        return BadRequest(response);
    }
}