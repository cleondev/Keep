using Keep.Application.Reminders;
using Keep.Domain.Entities;
using Keep.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Keep.Infrastructure.Services.Reminders;

public class ReminderService : IReminderService
{
    private readonly KeepDbContext _dbContext;

    public ReminderService(KeepDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyList<Reminder>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.Reminders
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<Reminder?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Reminders
            .AsNoTracking()
            .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);
    }

    public async Task<Reminder> CreateAsync(Reminder reminder, CancellationToken cancellationToken = default)
    {
        reminder.Id = reminder.Id == Guid.Empty ? Guid.NewGuid() : reminder.Id;

        _dbContext.Reminders.Add(reminder);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return reminder;
    }

    public async Task<bool> UpdateAsync(Reminder reminder, CancellationToken cancellationToken = default)
    {
        var existing = await _dbContext.Reminders.FirstOrDefaultAsync(r => r.Id == reminder.Id, cancellationToken);
        if (existing is null)
        {
            return false;
        }

        existing.RemindAt = reminder.RemindAt;
        existing.IsDone = reminder.IsDone;

        await _dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var reminder = await _dbContext.Reminders.FirstOrDefaultAsync(r => r.Id == id, cancellationToken);
        if (reminder is null)
        {
            return false;
        }

        _dbContext.Reminders.Remove(reminder);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }
}
