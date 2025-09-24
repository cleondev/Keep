using System;

namespace Keep.Domain.Entities;

public class NoteCollaborator
{
    public Guid NoteId { get; set; }
    public Guid CollaboratorId { get; set; }
    public bool CanEdit { get; set; } = true;

    public Note? Note { get; set; }
    public User? Collaborator { get; set; }
}
