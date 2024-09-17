using System.Security.Claims;
using DailyPlanner.Domain.DTO.User;
using DailyPlanner.Domain.Result;

namespace DailyPlanner.Domain.Interfaces.Services;

public interface ITokenService
{
    string GenerateAccessToken(IEnumerable<Claim> claims);
    string GenerateRefreshToken();
    ClaimsPrincipal GetPrincipalFromExpiredToken(string accessToken);
    Task<BaseResult<TokenDto>> RefreshToken(TokenDto tokenDto);
}