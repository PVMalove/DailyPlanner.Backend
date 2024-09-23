using AutoMapper;
using DailyPlanner.Application.Resources;
using DailyPlanner.Domain.DTO.Role;
using DailyPlanner.Domain.Entities;
using DailyPlanner.Domain.Enum;
using DailyPlanner.Domain.Interfaces.Repository;
using DailyPlanner.Domain.Interfaces.Services;
using DailyPlanner.Domain.Result;
using Microsoft.EntityFrameworkCore;

namespace DailyPlanner.Application.Services;

public class RoleService : IRoleService
{
    private readonly IBaseRepository<User> userRepository;
    private readonly IBaseRepository<Role> roleRepository;
    private readonly IBaseRepository<UserRole> userRoleRepository;
    private readonly IMapper mapper;
    

    public RoleService(IBaseRepository<User> userRepository, IBaseRepository<Role> roleRepository, 
        IBaseRepository<UserRole> userRoleRepository, IMapper mapper)
    {
        this.userRepository = userRepository;
        this.roleRepository = roleRepository;
        this.userRoleRepository = userRoleRepository;
        this.mapper = mapper;
    }
    
    /// <inheritdoc />
    public async Task<BaseResult<RoleDto>> CreateAsync(CreateRoleDto roleDto)
    {
        var role = await roleRepository.GetAll().AsNoTracking()
            .FirstOrDefaultAsync(r => r.Name == roleDto.Name);

        if (role is not null)
        {
            return new BaseResult<RoleDto>
            {
                ErrorMessage = ErrorMessage.RoleAlreadyExists,
                ErrorCode = ErrorCodes.RoleAlreadyExists
            };
        }

        role = new Role
        {
            Name = roleDto.Name
        };

        await roleRepository.CreateAsync(role);
        await roleRepository.SaveChangesAsync();

        return new BaseResult<RoleDto>
        {
            Data = mapper.Map<RoleDto>(role)
        };
    }

    /// <inheritdoc />
    public async Task<BaseResult<RoleDto>> UpdateAsync(RoleDto roleDto)
    {
        var role = await roleRepository.GetAll().AsNoTracking()
            .FirstOrDefaultAsync(r => r.Id == roleDto.Id);

        if (role is null)
        {
            return new BaseResult<RoleDto>
            {
                ErrorMessage = ErrorMessage.RoleNotFound,
                ErrorCode = ErrorCodes.RoleNotFound
            };
        }
        
        role.Name = roleDto.Name;
        roleRepository.Update(role);
        await roleRepository.SaveChangesAsync();
        
        return new BaseResult<RoleDto>
        {
            Data = mapper.Map<RoleDto>(role)
        };
    }

    /// <inheritdoc />
    public async Task<BaseResult<RoleDto>> DeleteAsync(long id)
    {
        var role = await roleRepository.GetAll().AsNoTracking()
            .FirstOrDefaultAsync(r => r.Id == id);

        if (role is null)
        {
            return new BaseResult<RoleDto>
            {
                ErrorMessage = ErrorMessage.RoleNotFound,
                ErrorCode = ErrorCodes.RoleNotFound
            };
        }
        
        roleRepository.Remove(role);
        await roleRepository.SaveChangesAsync();
        
        return new BaseResult<RoleDto>
        {
            Data = mapper.Map<RoleDto>(role)
        };
    }

    /// <inheritdoc />
    public async Task<BaseResult<UserRoleDto>> AddRoleForUserAsync(UserRoleDto userRoleDto)
    {
        var user = await userRepository.GetAll().AsNoTracking()
            .Include(u => u.Roles)
            .FirstOrDefaultAsync(u => u.Login == userRoleDto.Login);
        
        if (user is null)
        {
            return new BaseResult<UserRoleDto>
            {
                ErrorMessage = ErrorMessage.UserNotFound,
                ErrorCode = ErrorCodes.UserNotFound
            };
        }

        var roles = user.Roles.Select(r => r.Name).ToArray();

        if (!roles.Contains(userRoleDto.RoleName))
        {
            var role = await roleRepository.GetAll().AsNoTracking()
                .FirstOrDefaultAsync(r => r.Name == userRoleDto.RoleName);
            
            if (role is null)
            {
                return new BaseResult<UserRoleDto>
                {
                    ErrorMessage = ErrorMessage.RoleNotFound,
                    ErrorCode = ErrorCodes.RoleNotFound
                };
            }

            var userRole = new UserRole()
            {
                RoleId = role.Id,
                UserId = user.Id
            };

            await userRoleRepository.CreateAsync(userRole);
            await userRoleRepository.SaveChangesAsync();
            
            return new BaseResult<UserRoleDto>
            {
                Data = mapper.Map<UserRoleDto>(userRole)
            };
        }
        
        return new BaseResult<UserRoleDto>
        {
            ErrorMessage = ErrorMessage.UserAlreadyHasRole,
            ErrorCode = ErrorCodes.UserAlreadyHasRole
        };
    }
}