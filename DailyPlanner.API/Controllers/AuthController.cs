using Asp.Versioning;
using DailyPlanner.Domain.DTO.User;
using DailyPlanner.Domain.Interfaces.Services;
using DailyPlanner.Domain.Result;
using Microsoft.AspNetCore.Mvc;

namespace DailyPlanner.API.Controllers;

[ApiVersion("2.0")]
public class AuthController(IAuthService authService) : ApplicationController
{
    [HttpPost("register")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<BaseResult>> Register([FromBody] RegisterUserDto registerUserDto)
    {
        var response = await authService.Register(registerUserDto);
        if (response.IsSuccess)
        {
            return Ok(response);
        }
        return BadRequest(response);
    }
    
    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<TokenDto>> Login([FromBody] LoginUserDto loginUserDto)
    {
        var response = await authService.Login(loginUserDto);
        if (response.IsSuccess)
        {
            return Ok(response);
        }
        return BadRequest(response);
    }
}