using AutoMapper;
using DailyPlanner.Domain.DTO.Role;
using DailyPlanner.Domain.Entities;

namespace DailyPlanner.Application.Mapping;

public class UserRoleMapping: Profile
{
    public UserRoleMapping()
    {
        CreateMap<UserRole, UserRoleDto>().ReverseMap();
    }
}