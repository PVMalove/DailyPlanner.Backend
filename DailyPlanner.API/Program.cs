using DailyPlanner.API.Extensions;
using DailyPlanner.Application;
using DailyPlanner.Persistence;
using Serilog;


var builder = WebApplication.CreateBuilder(args);
IServiceCollection services = builder.Services;

builder.Host.UseSerilog((context , configuration)=>configuration.ReadFrom.Configuration(context.Configuration));
services.AddControllers();
services.AddPersistence(builder.Configuration);
services.AddApplication();
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    await app.DbInitializer();
}

app.UseHttpsRedirection();
app.MapControllers();
app.Run();
