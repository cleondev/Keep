using Keep.Application.Collaborators;
using Keep.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Keep.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CollaboratorsController : ControllerBase
{
    private readonly INoteCollaboratorService _collaboratorService;

    public CollaboratorsController(INoteCollaboratorService collaboratorService)
    {
        _collaboratorService = collaboratorService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<NoteCollaborator>>> GetCollaborators(CancellationToken cancellationToken)
    {
        var collaborators = await _collaboratorService.GetAllAsync(cancellationToken);
        return Ok(collaborators);
    }

    [HttpGet("{noteId:guid}/{collaboratorId:guid}")]
    public async Task<ActionResult<NoteCollaborator>> GetCollaborator(Guid noteId, Guid collaboratorId, CancellationToken cancellationToken)
    {
        var collaborator = await _collaboratorService.GetByIdAsync(noteId, collaboratorId, cancellationToken);
        if (collaborator is null)
        {
            return NotFound();
        }

        return Ok(collaborator);
    }

    [HttpPost]
    public async Task<ActionResult<NoteCollaborator>> Create(NoteCollaborator collaborator, CancellationToken cancellationToken)
    {
        var created = await _collaboratorService.AddAsync(collaborator, cancellationToken);
        if (!created)
        {
            return Conflict("Collaborator already added to note.");
        }

        return CreatedAtAction(nameof(GetCollaborator), new { collaborator.NoteId, collaborator.CollaboratorId }, collaborator);
    }

    [HttpDelete("{noteId:guid}/{collaboratorId:guid}")]
    public async Task<IActionResult> Delete(Guid noteId, Guid collaboratorId, CancellationToken cancellationToken)
    {
        var removed = await _collaboratorService.RemoveAsync(noteId, collaboratorId, cancellationToken);
        if (!removed)
        {
            return NotFound();
        }

        return NoContent();
    }
}
