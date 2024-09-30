using DailyPlanner.Domain.Entities;
using DailyPlanner.Domain.Interfaces.Repository;
using Microsoft.EntityFrameworkCore.Storage;

namespace DailyPlanner.Domain.Interfaces.Database;

public interface IUnitOfWork : IStateSaveChanges
{
    IBaseRepository<User> Users { get; }
    IBaseRepository<UserRole> UserRoles { get; }

    Task<IDbContextTransaction> BeginTransactionAsync();
}