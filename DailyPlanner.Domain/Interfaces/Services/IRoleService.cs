using DailyPlanner.Domain.DTO.Role;
using DailyPlanner.Domain.Entities;
using DailyPlanner.Domain.Result;

namespace DailyPlanner.Domain.Interfaces.Services;

public interface IRoleService
{
    /// <summary>
    /// Создание роли.
    /// </summary>
    /// <param name="roleDto"></param>
    /// <returns></returns>
    Task<BaseResult<Role>> CreateAsync(RoleDto roleDto);

    /// <summary>
    /// Обновление роли.
    /// </summary>
    /// <param name="roleDto"></param>
    /// <returns></returns>
    Task<BaseResult<Role>> UpdateAsync(RoleDto roleDto);
    
    /// <summary>
    /// Удаление роли.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<BaseResult<Role>> DeleteAsync(long id);
}