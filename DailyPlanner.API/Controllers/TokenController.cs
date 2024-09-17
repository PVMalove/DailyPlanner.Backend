using Asp.Versioning;
using DailyPlanner.Domain.DTO.User;
using DailyPlanner.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace DailyPlanner.API.Controllers;

[ApiVersion("2.0")]
public class TokenController(ITokenService tokenService) : ApplicationController
{
    [HttpPost("refresh")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<TokenDto>> RefreshToken([FromBody] TokenDto tokenDto)
    {
        var response = await tokenService.RefreshToken(tokenDto);
        if (response.IsSuccess)
        {
            return Ok(response);
        }
        return BadRequest(response);
    }
}