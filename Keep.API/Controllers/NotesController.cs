using Keep.Application.Notes;
using Keep.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Keep.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class NotesController : ControllerBase
{
    private readonly INoteService _noteService;

    public NotesController(INoteService noteService)
    {
        _noteService = noteService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Note>>> GetNotes(CancellationToken cancellationToken)
    {
        var notes = await _noteService.GetAllAsync(cancellationToken);
        return Ok(notes);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<Note>> GetNote(Guid id, CancellationToken cancellationToken)
    {
        var note = await _noteService.GetByIdAsync(id, cancellationToken);
        if (note is null)
        {
            return NotFound();
        }

        return Ok(note);
    }

    [HttpPost]
    public async Task<ActionResult<Note>> Create(Note note, CancellationToken cancellationToken)
    {
        var created = await _noteService.CreateAsync(note, cancellationToken);
        return CreatedAtAction(nameof(GetNote), new { id = created.Id }, created);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, Note updated, CancellationToken cancellationToken)
    {
        if (id != updated.Id)
        {
            return BadRequest();
        }

        var success = await _noteService.UpdateAsync(updated, cancellationToken);
        if (!success)
        {
            return NotFound();
        }

        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var success = await _noteService.SoftDeleteAsync(id, cancellationToken);
        if (!success)
        {
            return NotFound();
        }

        return NoContent();
    }
}
