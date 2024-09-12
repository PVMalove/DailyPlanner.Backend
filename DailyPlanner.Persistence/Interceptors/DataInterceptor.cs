using DailyPlanner.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace DailyPlanner.Persistence.Interceptors;

public class DataInterceptor : SaveChangesInterceptor
{
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result,
        CancellationToken cancellationToken = new CancellationToken())
    {
        var dbContext = eventData.Context;
        
        if (dbContext is null)
        {
            return base.SavingChangesAsync(eventData, result , cancellationToken);
        }

        var entries = dbContext.ChangeTracker.Entries<IAuditable>()
            .Where(x=>x.State is EntityState.Added or EntityState.Modified)
            .ToList();

        foreach (var entry in entries)
        {
            if (entry.State == EntityState.Added)
            {
                entry.Property(x=>x.CreatedAt).CurrentValue = DateTime.UtcNow;
            }

            if (entry.State == EntityState.Modified)
            {
                entry.Property(x=>x.UpdatedAt).CurrentValue = DateTime.UtcNow;
            }
        }
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        var dbContext = eventData.Context;
        
        if (dbContext is null)
        {
            return base.SavingChanges(eventData, result);
        }

        var entries = dbContext.ChangeTracker.Entries<IAuditable>()
            .Where(x=>x.State is EntityState.Added or EntityState.Modified)
            .ToList();

        foreach (var entry in entries)
        {
            UpdateAuditTimestamp(entry, DateTime.UtcNow);
        }
        
        return base.SavingChanges(eventData, result);
    }
    
    private void UpdateAuditTimestamp(EntityEntry<IAuditable> entity, DateTime timestamp)
    {
        if (entity.Entity is not { } auditableEntity) return;
        switch (entity.State)
        {
            case EntityState.Added:
                auditableEntity.CreatedAt = timestamp;
                break;
            case EntityState.Modified:
                auditableEntity.UpdatedAt = timestamp;
                break;
        }
    }
}