using DailyPlanner.Domain.Entities;
using DailyPlanner.Domain.Interfaces.Database;
using DailyPlanner.Domain.Interfaces.Repository;
using Microsoft.EntityFrameworkCore.Storage;

namespace DailyPlanner.Persistence.Database;

public class UnitOfWork : IUnitOfWork
{
    public IBaseRepository<User> Users { get; }
    public IBaseRepository<UserRole> UserRoles { get; }

    private readonly ApplicationDbContext context;

    public UnitOfWork(ApplicationDbContext context, IBaseRepository<User> users, IBaseRepository<UserRole> userRoles)
    {
        this.context = context;
        Users = users;
        UserRoles = userRoles;
    }

    public async Task<IDbContextTransaction> BeginTransactionAsync()
    {
        return await context.Database.BeginTransactionAsync(); 
    }

    public async Task<int> SaveChangesAsync()
    {
        return await context.SaveChangesAsync();
    }
}