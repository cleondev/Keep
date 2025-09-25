using Keep.Domain.Entities;

namespace Keep.Application.Collaborators;

public interface INoteCollaboratorService
{
    Task<IReadOnlyList<NoteCollaborator>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<NoteCollaborator?> GetByIdAsync(Guid noteId, Guid collaboratorId, CancellationToken cancellationToken = default);
    Task<bool> AddAsync(NoteCollaborator collaborator, CancellationToken cancellationToken = default);
    Task<bool> RemoveAsync(Guid noteId, Guid collaboratorId, CancellationToken cancellationToken = default);
}
