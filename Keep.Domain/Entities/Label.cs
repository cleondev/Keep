using System;
using System.Collections.Generic;

namespace Keep.Domain.Entities;

public class Label
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Name { get; set; } = string.Empty;

    public User? User { get; set; }
    public ICollection<NoteLabel> Notes { get; set; } = new List<NoteLabel>();
}
