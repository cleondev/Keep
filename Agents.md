# Agents.md

## 🎯 Mục tiêu
Xây dựng ứng dụng **Google Keep Clone** với các tính năng cốt lõi: Ghi chú, Nhắc nhở, Cộng tác, Gắn nhãn.

- **Kiến trúc**: Layered Architecture (.NET 9)
- **Database**: SQLite
- **UI**: React (tối giản)
- **Quản lý source**: Git

---

## 🏗️ Các bước thực hiện

### 1. Tạo cấu trúc dự án & cài đặt package
```bash
dotnet new sln -n KeepClone

# Tạo các layer
cd KeepClone
dotnet new classlib -n KeepClone.Domain
dotnet new classlib -n KeepClone.Application
dotnet new classlib -n KeepClone.Infrastructure
dotnet new webapi   -n KeepClone.API

dotnet sln add KeepClone.*

# Thêm reference
dotnet add KeepClone.Application reference KeepClone.Domain
dotnet add KeepClone.Infrastructure reference KeepClone.Application KeepClone.Domain
dotnet add KeepClone.API reference KeepClone.Application KeepClone.Infrastructure

# EF Core SQLite
dotnet add KeepClone.Infrastructure package Microsoft.EntityFrameworkCore

dotnet add KeepClone.Infrastructure package Microsoft.EntityFrameworkCore.Sqlite
dotnet add KeepClone.Infrastructure package Microsoft.EntityFrameworkCore.Design
```

### 2. Entity & DbContext

#### Entities (KeepClone.Domain/Entities)
```csharp
public class User
{
    public Guid Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public ICollection<Note> Notes { get; set; } = new List<Note>();
}

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

public class Reminder
{
    public Guid Id { get; set; }
    public Guid NoteId { get; set; }
    public DateTime RemindAt { get; set; }
    public bool IsDone { get; set; }
}

public class Label
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Name { get; set; } = string.Empty;
    public ICollection<NoteLabel> Notes { get; set; } = new List<NoteLabel>();
}

public class NoteLabel
{
    public Guid NoteId { get; set; }
    public Guid LabelId { get; set; }
}

public class NoteCollaborator
{
    public Guid NoteId { get; set; }
    public Guid CollaboratorId { get; set; }
    public bool CanEdit { get; set; } = true;
}
```

#### DbContext (KeepClone.Infrastructure)
```csharp
public class KeepDbContext : DbContext
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Note> Notes => Set<Note>();
    public DbSet<Reminder> Reminders => Set<Reminder>();
    public DbSet<Label> Labels => Set<Label>();

    public KeepDbContext(DbContextOptions<KeepDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<NoteLabel>().HasKey(x => new { x.NoteId, x.LabelId });
        modelBuilder.Entity<NoteCollaborator>().HasKey(x => new { x.NoteId, x.CollaboratorId });
    }
}
```

#### Init DB
```bash
dotnet ef migrations add Init
dotnet ef database update
```

### 3. API Controllers (KeepClone.API)

#### NotesController
```csharp
[ApiController]
[Route("api/[controller]")]
public class NotesController : ControllerBase
{
    private readonly KeepDbContext _db;
    public NotesController(KeepDbContext db) => _db = db;

    [HttpGet]
    public async Task<IEnumerable<Note>> GetNotes()
        => await _db.Notes.Include(n => n.Reminders).ToListAsync();

    [HttpPost]
    public async Task<IActionResult> Create(Note note)
    {
        _db.Notes.Add(note);
        await _db.SaveChangesAsync();
        return Ok(note);
    }
}
```

#### LabelsController
```csharp
[ApiController]
[Route("api/[controller]")]
public class LabelsController : ControllerBase
{
    private readonly KeepDbContext _db;
    public LabelsController(KeepDbContext db) => _db = db;

    [HttpPost]
    public async Task<IActionResult> Create(Label label)
    {
        _db.Labels.Add(label);
        await _db.SaveChangesAsync();
        return Ok(label);
    }
}
```

(Tương tự có thể viết cho `RemindersController` và `CollaboratorsController` theo cùng pattern CRUD.)

### 4. UI React (tối giản)
```bash
npm create vite@latest keep-ui --template react
cd keep-ui && npm install axios react-router-dom
```

#### NotesPage.jsx
```jsx
import axios from 'axios';
import { useEffect, useState } from 'react';

export default function NotesPage() {
  const [notes, setNotes] = useState([]);

  useEffect(() => {
    axios.get("http://localhost:5000/api/notes")
         .then(res => setNotes(res.data));
  }, []);

  return (
    <div>
      <h1>Notes</h1>
      {notes.map(n => <div key={n.id}>{n.title}</div>)}
    </div>
  );
}
```

---

## 🔧 Git Workflow

```bash
# Khởi tạo repo
git init
git add .
git commit -m "Init KeepClone project"

# Tạo branch feature
git checkout -b feature/notes-api

# Commit code
git add .
git commit -m "Add Notes API"

# Push lên remote
git remote add origin <repo-url>
git push -u origin feature/notes-api
```

Khi hoàn tất: tạo Pull Request merge vào `main`.

---

## ✅ Kết luận
- Codex có thể làm tuần tự: **Entity → Migration → API → UI → Git**.
- Chỉ giữ phần cần thiết, tránh dư thừa.
- Mỗi bước đều rõ ràng, dễ thực hiện.

