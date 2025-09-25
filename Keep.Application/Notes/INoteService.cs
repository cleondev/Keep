using Keep.Domain.Entities;

namespace Keep.Application.Notes;

public interface INoteService
{
    Task<IReadOnlyList<Note>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Note?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Note> CreateAsync(Note note, CancellationToken cancellationToken = default);
    Task<bool> UpdateAsync(Note note, CancellationToken cancellationToken = default);
    Task<bool> SoftDeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
