using Asp.Versioning.ApiExplorer;
using DailyPlanner.API.Extensions;
using DailyPlanner.API.Middlewares;
using Serilog;


var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog((context , configuration)=>configuration.ReadFrom.Configuration(context.Configuration));

IServiceCollection services = builder.Services;
services.AddCustomServices(builder);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    await app.DbInitializer();
    app.UseDeveloperExceptionPage();
    var apiVersionDescriptionProvider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
    app.UseSwagger();
    app.UseSwaggerUI(config =>
    {
        foreach (var description in apiVersionDescriptionProvider.ApiVersionDescriptions.Reverse())
        {
            config.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json",
                $"Daily Planner swagger {description.GroupName.ToUpperInvariant()}");
        }
    });
}

app.UseMiddleware<ExceptionMiddleware>();
app.UseCors(x=> x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();
app.MapControllers();
app.Run();
