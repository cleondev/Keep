using Keep.Domain.Entities;
using Keep.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Keep.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CollaboratorsController : ControllerBase
{
    private readonly KeepDbContext _db;

    public CollaboratorsController(KeepDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<NoteCollaborator>>> GetCollaborators()
    {
        var collaborators = await _db.NoteCollaborators
            .AsNoTracking()
            .ToListAsync();

        return Ok(collaborators);
    }

    [HttpGet("{noteId:guid}/{collaboratorId:guid}")]
    public async Task<ActionResult<NoteCollaborator>> GetCollaborator(Guid noteId, Guid collaboratorId)
    {
        var collaborator = await _db.NoteCollaborators
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.NoteId == noteId && c.CollaboratorId == collaboratorId);

        if (collaborator is null)
        {
            return NotFound();
        }

        return Ok(collaborator);
    }

    [HttpPost]
    public async Task<ActionResult<NoteCollaborator>> Create(NoteCollaborator collaborator)
    {
        var exists = await _db.NoteCollaborators
            .AnyAsync(c => c.NoteId == collaborator.NoteId && c.CollaboratorId == collaborator.CollaboratorId);

        if (exists)
        {
            return Conflict("Collaborator already added to note.");
        }

        _db.NoteCollaborators.Add(collaborator);
        await _db.SaveChangesAsync();

        return CreatedAtAction(nameof(GetCollaborator), new { collaborator.NoteId, collaborator.CollaboratorId }, collaborator);
    }

    [HttpDelete("{noteId:guid}/{collaboratorId:guid}")]
    public async Task<IActionResult> Delete(Guid noteId, Guid collaboratorId)
    {
        var collaborator = await _db.NoteCollaborators
            .FirstOrDefaultAsync(c => c.NoteId == noteId && c.CollaboratorId == collaboratorId);

        if (collaborator is null)
        {
            return NotFound();
        }

        _db.NoteCollaborators.Remove(collaborator);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}
