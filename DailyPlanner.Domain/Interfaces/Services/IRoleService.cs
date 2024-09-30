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
    Task<BaseResult<RoleDto>> CreateAsync(CreateRoleDto roleDto);

    /// <summary>
    /// Обновление роли.
    /// </summary>
    /// <param name="roleDto"></param>
    /// <returns></returns>
    Task<BaseResult<RoleDto>> UpdateAsync(RoleDto roleDto);
    
    /// <summary>
    /// Удаление роли.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<BaseResult<RoleDto>> DeleteAsync(long id);
    
    /// <summary>
    /// Добавление роли для пользователя.
    /// </summary>
    /// <param name="userRoleDto"></param>
    /// <returns></returns>
    Task<BaseResult<UserRoleDto>> AddRoleForUserAsync(UserRoleDto userRoleDto);
    
    /// <summary>
    /// Удаление роли у пользователя.
    /// </summary>
    /// <param name="userRoleDto"></param>
    /// <returns></returns>
    Task<BaseResult<UserRoleDto>> DeleteRoleForUserAsync(UserRoleDto userRoleDto);
}