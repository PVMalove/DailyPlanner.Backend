using Microsoft.Extensions.DependencyInjection;

namespace DailyPlanner.Consumer;

public static class Inject
{
    public static IServiceCollection AddConsumer(this IServiceCollection services)
    {
        services.AddHostedService<RabbitMqListener>();
        return services;
    }
}