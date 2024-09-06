﻿using DailyPlanner.Persistence;
using Microsoft.EntityFrameworkCore;

namespace DailyPlanner.API.Extensions;

public static class AppExtensions
{
    public static async Task DbInitializer(this WebApplication app)
    {
        await using var scope = app.Services.CreateAsyncScope();
        try
        {
            await using var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            await dbContext.Database.MigrateAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error initializing database: {ex.Message}");
            throw;
        }
    }
}