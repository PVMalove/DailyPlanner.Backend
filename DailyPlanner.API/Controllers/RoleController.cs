using System.Net.Mime;
using Asp.Versioning;
using DailyPlanner.Domain.DTO.Role;
using DailyPlanner.Domain.Entities;
using DailyPlanner.Domain.Interfaces.Services;
using DailyPlanner.Domain.Result;
using Microsoft.AspNetCore.Mvc;

namespace DailyPlanner.API.Controllers;


[ApiVersion("2.0")]
[Consumes(MediaTypeNames.Application.Json)]
public class RoleController(IRoleService roleService) : ApplicationController
{
    [HttpPost("create")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<BaseResult<Role>>> GetById([FromBody] CreateRoleDto roleDto)
    {
        var response = await roleService.CreateAsync(roleDto);
        if (response.IsSuccess)
        {
            return Ok(response);
        }
        return BadRequest(response);
    }
    
    [HttpPut("update")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
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