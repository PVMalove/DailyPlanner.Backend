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
    private readonly ILogger logger;
    private readonly IMapper mapper;

    public AuthService(IBaseRepository<User> userRepository, IMapper mapper, IBaseRepository<UserToken> userTokenRepository, ILogger logger)
    {
        this.userRepository = userRepository;
        this.mapper = mapper;
        this.userTokenRepository = userTokenRepository;
        this.logger = logger;
    }
    
    /// <inheritdoc />
    public async Task<BaseResult<UserDto>> Register(RegisterUserDto registerUserDto)
    {
        try
        {
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
        catch (Exception ex)
        {
            logger.Error(ex, ex.Message);
            return new BaseResult<UserDto>(
                errorMessage: ErrorMessage.PasswordsNotMatch,
                errorCode: ErrorCodes.PasswordsNotMatch);
        }
    }

    public async Task<BaseResult<TokenDto>> Login(LoginUserDto loginUserDto)
    {
        try
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
            
            if (userToken is null)
            {
                userToken = new UserToken
                {
                    UserId = user.Id
                };
                await userTokenRepository.CreateAsync(userToken);
                await userTokenRepository.SaveChangesAsync();
            }

        }
        catch (Exception ex)
        {
            logger.Error(ex, ex.Message);
            return new BaseResult<TokenDto>(
                errorMessage: ErrorMessage.InternalServerError,
                errorCode: ErrorCodes.InternalServerError);
        }
    }

    private string HashPassword(string password)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(password));
        return BitConverter.ToString(bytes);
    }
    
    private bool IsVerifyPassword(string userPasswordHash, string userPassword)
    {
        var hash = HashPassword(userPassword);
        return userPasswordHash.Equals(hash);
    }
}