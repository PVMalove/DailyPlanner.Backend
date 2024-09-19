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
    private IBaseRepository<User> userRepository;
    private IBaseRepository<Role> roleRepository;

    public RoleService(IBaseRepository<User> userRepository, IBaseRepository<Role> roleRepository)
    {
        this.userRepository = userRepository;
        this.roleRepository = roleRepository;
    }

    public async Task<BaseResult<Role>> CreateAsync(RoleDto roleDto)
    {
        var role = await roleRepository.GetAll().AsNoTracking()
            .FirstOrDefaultAsync(r => r.Name == roleDto.Name);

        if (role is not null)
        {
            return new BaseResult<Role>
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

        return new BaseResult<Role>
        {
            Data = role
        };
    }


    public async Task<BaseResult<Role>> UpdateAsync(RoleDto roleDto)
    {
        var role = await roleRepository.GetAll().AsNoTracking()
            .FirstOrDefaultAsync(r => r.Id == roleDto.Id);

        if (role is null)
        {
            return new BaseResult<Role>
            {
                ErrorMessage = ErrorMessage.RoleNotFound,
                ErrorCode = ErrorCodes.RoleNotFound
            };
        }
        
        role.Name = roleDto.Name;
        roleRepository.Update(role);
        await roleRepository.SaveChangesAsync();
        
        return new BaseResult<Role>
        {
            Data = role
        };
    }

    public async Task<BaseResult<Role>> DeleteAsync(long id)
    {
        var role = await roleRepository.GetAll().AsNoTracking()
            .FirstOrDefaultAsync(r => r.Id == id);

        if (role is null)
        {
            return new BaseResult<Role>
            {
                ErrorMessage = ErrorMessage.RoleNotFound,
                ErrorCode = ErrorCodes.RoleNotFound
            };
        }
        
        roleRepository.Remove(role);
        await roleRepository.SaveChangesAsync();
        
        return new BaseResult<Role>
        {
            Data = role
        };
    }
}