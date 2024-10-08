﻿using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using AutoMapper;
using DailyPlanner.Application.Resources;
using DailyPlanner.Domain.DTO.User;
using DailyPlanner.Domain.Entities;
using DailyPlanner.Domain.Enum;
using DailyPlanner.Domain.Interfaces.Database;
using DailyPlanner.Domain.Interfaces.Repository;
using DailyPlanner.Domain.Interfaces.Services;
using DailyPlanner.Domain.Result;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace DailyPlanner.Application.Services;

public class AuthService : IAuthService
{
    private readonly IUnitOfWork unitOfWork;
    private readonly IBaseRepository<User> userRepository;
    private readonly IBaseRepository<UserToken> userTokenRepository;
    private readonly IBaseRepository<Role> roleRepository;
    private readonly ITokenService tokenService;
    private readonly IMapper mapper;
    private readonly ILogger logger;

    public AuthService(IUnitOfWork unitOfWork, IBaseRepository<User> userRepository,
        IBaseRepository<UserToken> userTokenRepository, IBaseRepository<Role> roleRepository,
        ITokenService tokenService, IMapper mapper, ILogger logger)
    {
        this.unitOfWork = unitOfWork;
        this.userRepository = userRepository;
        this.userTokenRepository = userTokenRepository;
        this.roleRepository = roleRepository;
        this.tokenService = tokenService;
        this.mapper = mapper;
        this.logger = logger;
    }

    /// <inheritdoc />
    public async Task<BaseResult<UserDto>> Register(RegisterUserDto registerUserDto)
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

        await using (var transaction = await unitOfWork.BeginTransactionAsync())
        {
            try
            {
                user = new User
                {
                    Login = registerUserDto.Login,
                    Password = hashedPassword
                };

                await unitOfWork.Users.CreateAsync(user);
                await unitOfWork.SaveChangesAsync();
                
                var role = await roleRepository.GetAll().AsNoTracking()
                    .FirstOrDefaultAsync(r => r.Name == nameof(Roles.User));
        
                if (role is null)
                {
                    logger.Warning(ErrorMessage.RoleNotFound);
                    return new BaseResult<UserDto>(
                        errorMessage: ErrorMessage.RoleNotFound,
                        errorCode: ErrorCodes.RoleNotFound);
                }
        
                var userRole = new UserRole
                {
                    UserId = user.Id,
                    RoleId = role.Id
                };
                
                await unitOfWork.UserRoles.CreateAsync(userRole);
                
                await unitOfWork.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                logger.Warning(ex.Message);
                await transaction.RollbackAsync();
            }
        }
        
        return new BaseResult<UserDto>(mapper.Map<UserDto>(user));
    }

    /// <inheritdoc />
    public async Task<BaseResult<TokenDto>> Login(LoginUserDto loginUserDto)
    {
        var user = await userRepository.GetAll().AsNoTracking()
            .Include(u => u.Roles)
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

        var claims = new List<Claim>();

        claims.AddRange(user.Roles.Select(r => new Claim(ClaimTypes.Role, r.Name)));
        claims.Add(new Claim(ClaimTypes.Name, user.Login));
        claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
        
        var accessToken = tokenService.GenerateAccessToken(claims);
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