using DailyPlanner.Domain.Interfaces.Database;

namespace DailyPlanner.Domain.Interfaces.Repository;

public interface IBaseRepository<TEntity> : IStateSaveChanges
{
    IQueryable<TEntity> GetAll();
    Task<TEntity> CreateAsync(TEntity entity);
    TEntity Update(TEntity entity);
    void Remove(TEntity entity);
}