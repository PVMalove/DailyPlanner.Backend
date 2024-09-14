using DailyPlanner.Domain.DTO.User;
using DailyPlanner.Domain.Interfaces.Services;
using DailyPlanner.Domain.Result;
using Microsoft.AspNetCore.Mvc;

namespace DailyPlanner.API.Controllers;

public class AuthController(IAuthService authService) : ApplicationController
{
    private readonly IAuthService authService = authService;


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
    
    // [HttpPost("login")]
    // public async Task<ActionResult<TokenDto>> Login([FromBody] LoginUserDto loginUserDto)
    // {
    //     
    // }
}