using Keep.Application.Collaborators;
using Keep.Domain.Entities;
using Keep.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Keep.Infrastructure.Services.Collaborators;

public class NoteCollaboratorService : INoteCollaboratorService
{
    private readonly KeepDbContext _dbContext;

    public NoteCollaboratorService(KeepDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyList<NoteCollaborator>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.NoteCollaborators
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<NoteCollaborator?> GetByIdAsync(Guid noteId, Guid collaboratorId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.NoteCollaborators
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.NoteId == noteId && c.CollaboratorId == collaboratorId, cancellationToken);
    }

    public async Task<bool> AddAsync(NoteCollaborator collaborator, CancellationToken cancellationToken = default)
    {
        var exists = await _dbContext.NoteCollaborators
            .AnyAsync(c => c.NoteId == collaborator.NoteId && c.CollaboratorId == collaborator.CollaboratorId, cancellationToken);

        if (exists)
        {
            return false;
        }

        _dbContext.NoteCollaborators.Add(collaborator);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<bool> RemoveAsync(Guid noteId, Guid collaboratorId, CancellationToken cancellationToken = default)
    {
        var collaborator = await _dbContext.NoteCollaborators
            .FirstOrDefaultAsync(c => c.NoteId == noteId && c.CollaboratorId == collaboratorId, cancellationToken);

        if (collaborator is null)
        {
            return false;
        }

        _dbContext.NoteCollaborators.Remove(collaborator);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }
}
