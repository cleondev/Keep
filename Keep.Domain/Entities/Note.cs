using System;
using System.Collections.Generic;

namespace Keep.Domain.Entities;

public class Note
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public bool IsPinned { get; set; }
    public bool IsArchived { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public ICollection<Reminder> Reminders { get; set; } = new List<Reminder>();
    public ICollection<NoteLabel> Labels { get; set; } = new List<NoteLabel>();
    public ICollection<NoteCollaborator> Collaborators { get; set; } = new List<NoteCollaborator>();
}
