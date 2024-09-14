using DailyPlanner.Application.Mapping;
using DailyPlanner.Application.Services;
using DailyPlanner.Application.Validations;
using DailyPlanner.Application.Validations.FluentValidator;
using DailyPlanner.Domain.DTO.Report;
using DailyPlanner.Domain.Interfaces.Services;
using DailyPlanner.Domain.Interfaces.Validations;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace DailyPlanner.Application;

public static class Inject
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(ReportMapping));
        
        services.AddScoped<IReportValidator, ReportValidator>();
        services.AddScoped<IValidator<CreateReportDto>, CreateReportValidator>();
        services.AddScoped<IValidator<UpdateReportDto>, UpdateReportValidator>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IReportService, ReportService>();
        
        return services;
    }
}