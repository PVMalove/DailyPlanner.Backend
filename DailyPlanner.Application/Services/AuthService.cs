using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using AutoMapper;
using DailyPlanner.Application.Resources;
using DailyPlanner.Domain.DTO.User;
using DailyPlanner.Domain.Entities;
using DailyPlanner.Domain.Enum;
using DailyPlanner.Domain.Interfaces.Repository;
using DailyPlanner.Domain.Interfaces.Services;
using DailyPlanner.Domain.Result;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace DailyPlanner.Application.Services;

public class AuthService : IAuthService
{
    private readonly IBaseRepository<User> userRepository;
    private readonly IBaseRepository<UserToken> userTokenRepository;
    private readonly ITokenService tokenService;
    private readonly IMapper mapper;
    private readonly ILogger logger;

    public AuthService(IBaseRepository<User> userRepository, IBaseRepository<UserToken> userTokenRepository,
        ITokenService tokenService, IMapper mapper, ILogger logger)
    {
        this.userRepository = userRepository;
        this.userTokenRepository = userTokenRepository;
        this.tokenService = tokenService;
        this.mapper = mapper;
        this.logger = logger;
    }

    /// <inheritdoc />
    public async Task<BaseResult<UserDto>> Register(RegisterUserDto registerUserDto)
    {
        throw new UnauthorizedAccessException("ssssss ssssssss ");
        
        if (!registerUserDto.Password.Equals(registerUserDto.PasswordConfirm))
        {
            logger.Warning(ErrorMessage.PasswordsNotMatch);
            return new BaseResult<UserDto>(
                errorMessage: ErrorMessage.PasswordsNotMatch,
                errorCode: ErrorCodes.PasswordsNotMatch);
        }

        var user = await userRepository.GetAll().AsNoTracking()
            .FirstOrDefaultAsync(u => u.Login == registerUserDto.Login);

        if (user != null)
        {
            logger.Warning(ErrorMessage.UserAlreadyExists);
            return new BaseResult<UserDto>(
                errorMessage: ErrorMessage.UserAlreadyExists,
                errorCode: ErrorCodes.UserAlreadyExists);
        }

        var hashedPassword = HashPassword(registerUserDto.Password);

        var newUser = new User
        {
            Login = registerUserDto.Login,
            Password = hashedPassword
        };

        await userRepository.CreateAsync(newUser);
        await userRepository.SaveChangesAsync();
        return new BaseResult<UserDto>(mapper.Map<UserDto>(newUser));
    }

    /// <inheritdoc />
    public async Task<BaseResult<TokenDto>> Login(LoginUserDto loginUserDto)
    {
        var user = await userRepository.GetAll().AsNoTracking()
            .FirstOrDefaultAsync(u => u.Login == loginUserDto.Login);

        if (user is null)
        {
            logger.Warning(ErrorMessage.UserNotFound);
            return new BaseResult<TokenDto>(
                errorMessage: ErrorMessage.UserNotFound,
                errorCode: ErrorCodes.UserNotFound);
        }

        if (!IsVerifyPassword(user.Password, loginUserDto.Password))
        {
            logger.Warning(ErrorMessage.PasswordIsNotCorrect);
            return new BaseResult<TokenDto>(
                errorMessage: ErrorMessage.PasswordIsNotCorrect,
                errorCode: ErrorCodes.PasswordIsNotCorrect);
        }

        var userToken = await userTokenRepository.GetAll().AsNoTracking()
            .FirstOrDefaultAsync(t => t.UserId == user.Id);

        var accessToken = tokenService.GenerateAccessToken(new List<Claim>
        {
            new(ClaimTypes.Name, user.Login),
            new(ClaimTypes.Role, "User")
        });

        var refreshToken = tokenService.GenerateRefreshToken();

        if (userToken is null)
        {
            userToken = new UserToken
            {
                UserId = user.Id,
                RefreshToken = refreshToken,
                RefreshTokenExpireTime = DateTime.UtcNow.AddDays(7)
            };
            await userTokenRepository.CreateAsync(userToken);
            await userTokenRepository.SaveChangesAsync();
        }
        else
        {
            userToken.RefreshToken = refreshToken;
            userToken.RefreshTokenExpireTime = DateTime.UtcNow.AddDays(7);
            userTokenRepository.Update(userToken);
            await userTokenRepository.SaveChangesAsync();
        }

        return new BaseResult<TokenDto>(new TokenDto
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken
        });
    }

    private string HashPassword(string password)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(password));
        return Convert.ToBase64String(bytes);
    }

    private bool IsVerifyPassword(string userPasswordHash, string userPassword)
    {
        var hash = HashPassword(userPassword);
        return userPasswordHash.Equals(hash);
    }
}