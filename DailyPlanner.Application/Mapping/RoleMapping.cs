using AutoMapper;
using DailyPlanner.Domain.DTO.Role;
using DailyPlanner.Domain.Entities;

namespace DailyPlanner.Application.Mapping;

public class RoleMapping : Profile
{
    public RoleMapping()
    {
        CreateMap<Role, RoleDto>().ReverseMap();
    }
}