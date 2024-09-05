using DailyPlanner.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace DailyPlanner.Persistence.Interceptors;

public class DataInterceptor : SaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        var dbContext = eventData.Context;
        
        if (dbContext is null)
        {
            return base.SavingChanges(eventData, result);
        }

        var entries = dbContext.ChangeTracker.Entries<IAuditable>();

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