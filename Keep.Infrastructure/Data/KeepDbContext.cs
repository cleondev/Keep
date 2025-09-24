using System;
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

        var createdAt = new DateTime(2024, 01, 01, 12, 0, 0, DateTimeKind.Utc);

        var aliceId = Guid.Parse("c0a1e8de-6ecf-4e55-8e55-291972c8aac8");
        var bobId = Guid.Parse("b4dff2fe-8f84-4db5-b5b3-85a5ae4dd9d9");

        var gardenNoteId = Guid.Parse("f5e9c5ad-8f53-4fb8-b2d2-b0c4f6690fa3");
        var groceryNoteId = Guid.Parse("5c2dbbfa-b9f0-4d11-b6eb-1cba574bf948");
        var launchNoteId = Guid.Parse("a3863d77-26cc-4d23-8a6a-5b4ad0b193b6");

        var personalLabelId = Guid.Parse("3f325968-7cd2-4d03-b17f-5ad5a121c5a6");
        var workLabelId = Guid.Parse("a3a873cb-76f6-4d8a-9e4f-1ff739218f58");
        var errandsLabelId = Guid.Parse("1a5da2ae-5f71-4479-b0f0-6763fec8db7c");

        var groceryReminderId = Guid.Parse("c3a91826-4460-4d9a-a685-5c4ed588db8c");
        var launchReminderId = Guid.Parse("2c3b9446-6431-4b6c-b4f4-f351b6463eb4");

        modelBuilder.Entity<User>().HasData(
            new User
            {
                Id = aliceId,
                Email = "alice@example.com",
                Name = "Alice Johnson",
                PasswordHash = "hashed-password-alice",
                CreatedAt = createdAt
            },
            new User
            {
                Id = bobId,
                Email = "bob@example.com",
                Name = "Bob Smith",
                PasswordHash = "hashed-password-bob",
                CreatedAt = createdAt.AddMinutes(15)
            });

        modelBuilder.Entity<Note>().HasData(
            new Note
            {
                Id = gardenNoteId,
                UserId = aliceId,
                Title = "Plan spring garden",
                Content = "Sketch layout, choose vegetables, order seeds",
                IsPinned = true,
                IsArchived = false,
                IsDeleted = false,
                CreatedAt = createdAt,
                UpdatedAt = createdAt
            },
            new Note
            {
                Id = groceryNoteId,
                UserId = aliceId,
                Title = "Saturday grocery run",
                Content = "Milk, bread, fruit, snacks for movie night",
                IsPinned = false,
                IsArchived = false,
                IsDeleted = false,
                CreatedAt = createdAt.AddDays(1),
                UpdatedAt = createdAt.AddDays(1)
            },
            new Note
            {
                Id = launchNoteId,
                UserId = bobId,
                Title = "Product launch checklist",
                Content = "Finalize messaging, schedule social posts, prep demo",
                IsPinned = false,
                IsArchived = false,
                IsDeleted = false,
                CreatedAt = createdAt.AddDays(2),
                UpdatedAt = createdAt.AddDays(2)
            });

        modelBuilder.Entity<Reminder>().HasData(
            new Reminder
            {
                Id = groceryReminderId,
                NoteId = groceryNoteId,
                RemindAt = createdAt.AddDays(1).AddHours(3),
                IsDone = false
            },
            new Reminder
            {
                Id = launchReminderId,
                NoteId = launchNoteId,
                RemindAt = createdAt.AddDays(2).AddHours(6),
                IsDone = false
            });

        modelBuilder.Entity<Label>().HasData(
            new Label
            {
                Id = personalLabelId,
                UserId = aliceId,
                Name = "Personal"
            },
            new Label
            {
                Id = errandsLabelId,
                UserId = aliceId,
                Name = "Errands"
            },
            new Label
            {
                Id = workLabelId,
                UserId = bobId,
                Name = "Work"
            });

        modelBuilder.Entity<NoteLabel>().HasData(
            new NoteLabel { NoteId = gardenNoteId, LabelId = personalLabelId },
            new NoteLabel { NoteId = groceryNoteId, LabelId = errandsLabelId },
            new NoteLabel { NoteId = launchNoteId, LabelId = workLabelId });

        modelBuilder.Entity<NoteCollaborator>().HasData(
            new NoteCollaborator
            {
                NoteId = groceryNoteId,
                CollaboratorId = bobId,
                CanEdit = false
            },
            new NoteCollaborator
            {
                NoteId = launchNoteId,
                CollaboratorId = aliceId,
                CanEdit = true
            });
    }
}
