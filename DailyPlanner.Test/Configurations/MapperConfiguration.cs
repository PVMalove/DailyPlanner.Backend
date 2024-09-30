using AutoMapper;
using DailyPlanner.Application.Mapping;

namespace DailyPlanner.Test.Configurations;

public static class MapperConfiguration
{
    public static IMapper GetMapperConfiguration()
    {
        var mockMapper = new AutoMapper.MapperConfiguration(cfg => { cfg.AddProfile(new ReportMapping()); });
        return mockMapper.CreateMapper();
    }
}