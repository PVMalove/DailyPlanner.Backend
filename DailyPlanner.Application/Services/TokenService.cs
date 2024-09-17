using System.IdentityModel.Tokens.Jwt;
using System.Security;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using DailyPlanner.Application.Resources;
using DailyPlanner.Domain.DTO.User;
using DailyPlanner.Domain.Entities;
using DailyPlanner.Domain.Interfaces.Repository;
using DailyPlanner.Domain.Interfaces.Services;
using DailyPlanner.Domain.Result;
using DailyPlanner.Domain.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace DailyPlanner.Application.Services;

public class TokenService : ITokenService
{
    private readonly IBaseRepository<User> userRepository;
    private readonly string jwtKey;
    private readonly string issuer;
    private readonly string audience;

    public TokenService(IBaseRepository<User> userRepository, IOptions<JwtSettings> options)
    {
        this.userRepository = userRepository;
        jwtKey = options.Value.JwtKey;
        issuer = options.Value.Issuer;
        audience = options.Value.Audience;
    }

    public string GenerateAccessToken(IEnumerable<Claim> claims)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        var securityToken = new JwtSecurityToken(issuer, audience, claims,null, DateTime.UtcNow.AddMinutes(10), credentials);
        var token = new JwtSecurityTokenHandler().WriteToken(securityToken);
        return token;
    }

    public string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using var randomNumberGenerator = RandomNumberGenerator.Create();
        randomNumberGenerator.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

    public ClaimsPrincipal GetPrincipalFromExpiredToken(string accessToken)
    {
        var tokenValidationParameters = new TokenValidationParameters()
        {
            ValidateAudience = true,
            ValidateIssuer = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
            ValidateLifetime = true
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        var claimsPrincipal = tokenHandler.ValidateToken(accessToken, tokenValidationParameters, out var securityToken);
        if (securityToken is not JwtSecurityToken jwtSecurityToken ||
            !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                StringComparison.InvariantCultureIgnoreCase))
        {
            throw new SecurityException("Invalid token");
        }

        return claimsPrincipal;
    }

    public async Task<BaseResult<TokenDto>> RefreshToken(TokenDto tokenDto)
    {
        var accessToken = tokenDto.AccessToken;
        var refreshToken = tokenDto.RefreshToken;

        var claimsPrincipal = GetPrincipalFromExpiredToken(accessToken);
        var userName = claimsPrincipal.Identity?.Name;

        var user = await userRepository.GetAll()
            .Include(x => x.UserToken)
            .FirstOrDefaultAsync(x => x.Login == userName);
        
        if (user == null || user.UserToken.RefreshToken != refreshToken ||
            user.UserToken.RefreshTokenExpireTime <= DateTime.UtcNow)
        {
            return new BaseResult<TokenDto>()
            {
                ErrorMessage = ErrorMessage.InvalidClientRequest
            };
        }

        var newAccessToken = GenerateAccessToken(claimsPrincipal.Claims);
        var newRefreshToken = GenerateRefreshToken();

        user.UserToken.RefreshToken = newRefreshToken;
        
        userRepository.Update(user);
        await userRepository.SaveChangesAsync();

        return new BaseResult<TokenDto>()
        {
            Data = new TokenDto()
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken
            }
        };
    }
}