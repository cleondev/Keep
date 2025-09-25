using Keep.Application.Users;
using Keep.Domain.Entities;
using Keep.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Keep.Infrastructure.Services.Users;

public class UserService : IUserService
{
    private readonly KeepDbContext _dbContext;

    public UserService(KeepDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyList<User>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.Users
            .AsNoTracking()
            .Include(u => u.Notes)
            .ToListAsync(cancellationToken);
    }

    public async Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Users
            .AsNoTracking()
            .Include(u => u.Notes)
            .FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
    }

    public async Task<User> CreateAsync(User user, CancellationToken cancellationToken = default)
    {
        user.Id = user.Id == Guid.Empty ? Guid.NewGuid() : user.Id;
        user.CreatedAt = DateTime.UtcNow;

        _dbContext.Users.Add(user);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return user;
    }
}
