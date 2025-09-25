using Keep.Domain.Entities;

namespace Keep.Application.Reminders;

public interface IReminderService
{
    Task<IReadOnlyList<Reminder>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Reminder?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Reminder> CreateAsync(Reminder reminder, CancellationToken cancellationToken = default);
    Task<bool> UpdateAsync(Reminder reminder, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
