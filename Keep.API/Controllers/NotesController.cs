using Keep.Domain.Entities;
using Keep.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Keep.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class NotesController : ControllerBase
{
    private readonly KeepDbContext _db;

    public NotesController(KeepDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Note>>> GetNotes()
    {
        var notes = await _db.Notes
            .AsNoTracking()
            .Include(n => n.Reminders)
            .Include(n => n.Labels).ThenInclude(nl => nl.Label)
            .Include(n => n.Collaborators).ThenInclude(c => c.Collaborator)
            .ToListAsync();

        return Ok(notes);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<Note>> GetNote(Guid id)
    {
        var note = await _db.Notes
            .AsNoTracking()
            .Include(n => n.Reminders)
            .Include(n => n.Labels).ThenInclude(nl => nl.Label)
            .Include(n => n.Collaborators).ThenInclude(c => c.Collaborator)
            .FirstOrDefaultAsync(n => n.Id == id);

        if (note is null)
        {
            return NotFound();
        }

        return Ok(note);
    }

    [HttpPost]
    public async Task<ActionResult<Note>> Create(Note note)
    {
        note.Id = Guid.NewGuid();
        note.CreatedAt = DateTime.UtcNow;
        note.UpdatedAt = DateTime.UtcNow;

        _db.Notes.Add(note);
        await _db.SaveChangesAsync();

        return CreatedAtAction(nameof(GetNote), new { id = note.Id }, note);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, Note updated)
    {
        if (id != updated.Id)
        {
            return BadRequest();
        }

        var note = await _db.Notes.FirstOrDefaultAsync(n => n.Id == id);
        if (note is null)
        {
            return NotFound();
        }

        note.Title = updated.Title;
        note.Content = updated.Content;
        note.IsPinned = updated.IsPinned;
        note.IsArchived = updated.IsArchived;
        note.IsDeleted = updated.IsDeleted;
        note.UpdatedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var note = await _db.Notes.FirstOrDefaultAsync(n => n.Id == id);
        if (note is null)
        {
            return NotFound();
        }

        note.IsDeleted = true;
        note.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();
        return NoContent();
    }
}
