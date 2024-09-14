using AutoMapper;
using DailyPlanner.Domain.DTO.User;
using DailyPlanner.Domain.Entities;

namespace DailyPlanner.Application.Mapping;

public class UserMapping : Profile
{
    public UserMapping()
    {
        CreateMap<User, UserDto>();
    }
}