using Keep.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Keep.Infrastructure.Data;

public class KeepDbContext : DbContext
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Note> Notes => Set<Note>();
    public DbSet<Reminder> Reminders => Set<Reminder>();
    public DbSet<Label> Labels => Set<Label>();
    public DbSet<NoteLabel> NoteLabels => Set<NoteLabel>();
    public DbSet<NoteCollaborator> NoteCollaborators => Set<NoteCollaborator>();

    public KeepDbContext(DbContextOptions<KeepDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<NoteLabel>().HasKey(x => new { x.NoteId, x.LabelId });
        modelBuilder.Entity<NoteCollaborator>().HasKey(x => new { x.NoteId, x.CollaboratorId });
    }
}
