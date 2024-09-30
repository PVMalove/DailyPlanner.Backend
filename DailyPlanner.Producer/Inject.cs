using DailyPlanner.Producer.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DailyPlanner.Producer;

public static class Inject
{
    public static IServiceCollection AddProducer(this IServiceCollection services)
    {
        services.AddScoped<IMessageProducer, Producer>();
        return services;
    }
}