using AutoMapper;
using DailyPlanner.Domain.DTO.Report;
using DailyPlanner.Domain.Entities;

namespace DailyPlanner.Application.Mapping;

public class ReportMapping : Profile
{
    protected ReportMapping()
    {
        CreateMap<Report, ReportDto>().ReverseMap();
    }
}