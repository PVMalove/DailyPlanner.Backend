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
public class RoleController : ApplicationController
{
    private IRoleService roleService;

    public RoleController(IRoleService roleService)
    {
        this.roleService = roleService;
    }
    
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<BaseResult<Role>>> GetById([FromBody] RoleDto roleDto)
    {
        var response = await roleService.CreateAsync(roleDto);
        if (response.IsSuccess)
        {
            return Ok(response);
        }
        return BadRequest(response);
    }
    
    [HttpPut]
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
    
    [HttpDelete("{id}")]
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
}