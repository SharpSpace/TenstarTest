using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Repositories;

public sealed class UserRepository(ILogger<UserRepository> logger, AppDbContext context) : IUserRepository
{
    public async Task AddAsync(IEnumerable<User> users)
    {
        try
        {
            await context.Users.AddRangeAsync(users);
            await context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            var dbPath = context.Database.GetDbConnection().DataSource;
            var fullDbPath = Path.GetFullPath(dbPath);
            logger.LogError(ex, "Failed to add users. SQLite DB path: {FullDbPath}", fullDbPath);
            logger.LogError("Exception: {Message}", ex.Message);
            throw;
        }
    }

    public async Task AddAsync(User user)
    {
        try
        {
            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            var dbPath = context.Database.GetDbConnection().DataSource;
            var fullDbPath = Path.GetFullPath(dbPath);
            logger.LogError(ex, "Failed to add user. SQLite DB path: {FullDbPath}", fullDbPath);
            logger.LogError("Exception: {Message}", ex.Message);
            throw;
        }
    }

    public async Task<IEnumerable<User>> GetAllAsync() => await context.Users.ToListAsync();

    public async Task<User?> GetByIdAsync(int id) => await context.Users.FindAsync(id);
}