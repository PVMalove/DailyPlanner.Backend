using System.Text;
using Asp.Versioning;
using DailyPlanner.API.Configurations;
using DailyPlanner.Application;
using DailyPlanner.Consumer;
using DailyPlanner.Domain.Settings;
using DailyPlanner.Persistence;
using DailyPlanner.Producer;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace DailyPlanner.API.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection AddCustomServices(this IServiceCollection services, WebApplicationBuilder builder)
    {
        builder.Services.Configure<RabbitMqSettings>(builder.Configuration.GetSection(nameof(RabbitMqSettings)));
        builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection(JwtSettings.DefaultSection));
        
        services.AddControllers();
        services.AddPersistence(builder.Configuration);
        services.AddApplication();

        services.AddAuthorization();
        services.AddAuthentication(config =>
        {
            config.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            config.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            config.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        })
            .AddJwtBearer("Bearer", options =>
        {
            var settings = builder.Configuration.GetSection(JwtSettings.DefaultSection).Get<JwtSettings>();
            var jwtKey = settings.JwtKey;
            var issuer = settings.Issuer;
            var audience = settings.Audience;
            options.Authority = settings.Authority;
            options.RequireHttpsMetadata = false;
            options.TokenValidationParameters = new TokenValidationParameters()
            {
                ValidIssuer = issuer,
                ValidAudience = audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
                ValidateAudience = true,
                ValidateIssuer = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true
            };
        });
        
        services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
        
        services.AddApiVersioning()
            .AddApiExplorer(options =>
            {
                options.DefaultApiVersion = new ApiVersion(1.0);
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
                options.AssumeDefaultVersionWhenUnspecified = true;
            });
        
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        services.AddProducer();
        services.AddConsumer();
        
        return services;
    }
}