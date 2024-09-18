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
    
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Issuer = issuer,
            Audience = audience,
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(10),
            SigningCredentials = credentials
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var encodedJwt = tokenHandler.CreateEncodedJwt(tokenDescriptor);
        return encodedJwt;
    }

    public string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber).Substring(0, 32);
    }

    public async Task<BaseResult<TokenDto>> RefreshToken(TokenDto tokenDto)
    {
        if (!IsValidRefreshToken(tokenDto.RefreshToken))
            return new BaseResult<TokenDto> { ErrorMessage = ErrorMessage.InvalidClientRequest };

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
            return new BaseResult<TokenDto> { ErrorMessage = ErrorMessage.InvalidClientRequest };
        }

        var newAccessToken = GenerateAccessToken(claimsPrincipal.Claims);
        var newRefreshToken = GenerateRefreshToken();

        user.UserToken.RefreshToken = newRefreshToken;
        
        userRepository.Update(user);
        await userRepository.SaveChangesAsync();

        return new BaseResult<TokenDto>
        {
            Data = new TokenDto
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken
            }
        };
    }

    private ClaimsPrincipal GetPrincipalFromExpiredToken(string accessToken)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = true,
            ValidateIssuer = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
            ValidateLifetime = true,
            ValidAudience = audience,
            ValidIssuer = issuer,
            ClockSkew = TimeSpan.Zero
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        try
        {
            return tokenHandler.ValidateToken(accessToken, tokenValidationParameters, out _);
        }
        catch (SecurityException)
        {
            throw new SecurityException("Неверный токен");
        }
    }

    private bool IsValidRefreshToken(string refreshToken)
    {
        return !string.IsNullOrEmpty(refreshToken) && refreshToken.Length == 32;
    }
}