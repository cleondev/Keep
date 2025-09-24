using Keep.Domain.Entities;
using Keep.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Keep.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LabelsController : ControllerBase
{
    private readonly KeepDbContext _db;

    public LabelsController(KeepDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Label>>> GetLabels()
    {
        var labels = await _db.Labels
            .AsNoTracking()
            .Include(l => l.Notes)
            .ToListAsync();

        return Ok(labels);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<Label>> GetLabel(Guid id)
    {
        var label = await _db.Labels
            .AsNoTracking()
            .Include(l => l.Notes)
            .FirstOrDefaultAsync(l => l.Id == id);

        if (label is null)
        {
            return NotFound();
        }

        return Ok(label);
    }

    [HttpPost]
    public async Task<ActionResult<Label>> Create(Label label)
    {
        label.Id = Guid.NewGuid();

        _db.Labels.Add(label);
        await _db.SaveChangesAsync();

        return CreatedAtAction(nameof(GetLabel), new { id = label.Id }, label);
    }
}
