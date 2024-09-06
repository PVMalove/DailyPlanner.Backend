using DailyPlanner.API.Extensions;
using DailyPlanner.Persistence;

var builder = WebApplication.CreateBuilder(args);
IServiceCollection services = builder.Services;

services.AddPersistence(builder.Configuration);
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
app.Run();
