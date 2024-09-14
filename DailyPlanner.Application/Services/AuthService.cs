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
    private readonly ILogger logger;
    private readonly IMapper mapper;

    public AuthService(IBaseRepository<User> userRepository, IMapper mapper, ILogger logger)
    {
        this.userRepository = userRepository;
        this.mapper = mapper;
        this.logger = logger;
    }
    
    /// <inheritdoc />
    public async Task<BaseResult<UserDto>> Register(RegisterUserDto registerUserDto)
    {
        try
        {
            if (registerUserDto.Password.Equals(registerUserDto.PasswordConfirm))
            {
                var user = userRepository.GetAll().AsNoTracking()
                    .FirstOrDefaultAsync(u => u.Login == registerUserDto.Login);

                if (user.Result is not null)
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

            logger.Warning(ErrorMessage.PasswordsNotMatch);
            return new BaseResult<UserDto>(
                errorMessage: ErrorMessage.PasswordsNotMatch,
                errorCode: ErrorCodes.PasswordsNotMatch);
        }
        catch (Exception ex)
        {
            logger.Error(ex, ex.Message);
            return new BaseResult<UserDto>(
                errorMessage: ErrorMessage.PasswordsNotMatch,
                errorCode: ErrorCodes.PasswordsNotMatch);
        }
    }

    public Task<BaseResult<TokenDto>> Login(LoginUserDto loginUserDto)
    {
        throw new NotImplementedException();
    }

    private string HashPassword(string password)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(password));
        return BitConverter.ToString(bytes);
    }
}