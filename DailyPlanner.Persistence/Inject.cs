using DailyPlanner.Domain.Entities;
using DailyPlanner.Domain.Interfaces.Repository;
using DailyPlanner.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DailyPlanner.Persistence;

public static class Inject
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DbConnection");
        services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(connectionString));
        services.AddScoped<IBaseRepository<User>, BaseRepository<User>>();
        services.AddScoped<IBaseRepository<Report>, BaseRepository<Report>>();
        services.AddScoped<IBaseRepository<UserToken>, BaseRepository<UserToken>>();
        services.AddScoped<IBaseRepository<Role>, BaseRepository<Role>>();
        return services;
    }
}