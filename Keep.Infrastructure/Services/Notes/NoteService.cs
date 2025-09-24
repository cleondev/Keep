using Keep.Application.Notes;
using Keep.Domain.Entities;
using Keep.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Keep.Infrastructure.Services.Notes;

public class NoteService : INoteService
{
    private readonly KeepDbContext _dbContext;

    public NoteService(KeepDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyList<Note>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.Notes
            .AsNoTracking()
            .Include(n => n.Reminders)
            .Include(n => n.Labels)
            .Include(n => n.Collaborators)
            .ToListAsync(cancellationToken);
    }

    public async Task<Note?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Notes
            .AsNoTracking()
            .Include(n => n.Reminders)
            .Include(n => n.Labels)
            .Include(n => n.Collaborators)
            .FirstOrDefaultAsync(n => n.Id == id, cancellationToken);
    }

    public async Task<Note> CreateAsync(Note note, CancellationToken cancellationToken = default)
    {
        note.Id = note.Id == Guid.Empty ? Guid.NewGuid() : note.Id;
        note.CreatedAt = DateTime.UtcNow;
        note.UpdatedAt = DateTime.UtcNow;

        _dbContext.Notes.Add(note);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return note;
    }

    public async Task<bool> UpdateAsync(Note note, CancellationToken cancellationToken = default)
    {
        var existing = await _dbContext.Notes.FirstOrDefaultAsync(n => n.Id == note.Id, cancellationToken);
        if (existing is null)
        {
            return false;
        }

        existing.Title = note.Title;
        existing.Content = note.Content;
        existing.IsPinned = note.IsPinned;
        existing.IsArchived = note.IsArchived;
        existing.IsDeleted = note.IsDeleted;
        existing.UpdatedAt = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<bool> SoftDeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var note = await _dbContext.Notes.FirstOrDefaultAsync(n => n.Id == id, cancellationToken);
        if (note is null)
        {
            return false;
        }

        note.IsDeleted = true;
        note.UpdatedAt = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }
}
