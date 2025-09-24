using System;

namespace Keep.Domain.Entities;

public class Reminder
{
    public Guid Id { get; set; }
    public Guid NoteId { get; set; }
    public DateTime RemindAt { get; set; }
    public bool IsDone { get; set; }
}
