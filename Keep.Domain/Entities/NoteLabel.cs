using System;

namespace Keep.Domain.Entities;

public class NoteLabel
{
    public Guid NoteId { get; set; }
    public Guid LabelId { get; set; }
}
