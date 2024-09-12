using DailyPlanner.Domain.Interfaces.Repository;

namespace DailyPlanner.Persistence.Repositories;

public class BaseRepository<TEntity>(ApplicationDbContext dbContext) : IBaseRepository<TEntity>
    where TEntity : class
{
    public IQueryable<TEntity> GetAll()
    {
        return dbContext.Set<TEntity>();
    }

    public async Task<TEntity> CreateAsync(TEntity entity)
    {
        ArgumentNullException.ThrowIfNull(entity);
        await dbContext.AddAsync(entity);
        return entity;
    }

    public TEntity Update(TEntity entity)
    {
        ArgumentNullException.ThrowIfNull(entity);
        dbContext.Update(entity);
        return entity;
    }

    public void Remove(TEntity entity)
    {
        ArgumentNullException.ThrowIfNull(entity);
        dbContext.Remove(entity);
    }
    
    public async Task<int> SaveChangesAsync()
    {
        return await dbContext.SaveChangesAsync();
    }
}