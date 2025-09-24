using Keep.Application.Labels;
using Keep.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Keep.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LabelsController : ControllerBase
{
    private readonly ILabelService _labelService;

    public LabelsController(ILabelService labelService)
    {
        _labelService = labelService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Label>>> GetLabels(CancellationToken cancellationToken)
    {
        var labels = await _labelService.GetAllAsync(cancellationToken);
        return Ok(labels);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<Label>> GetLabel(Guid id, CancellationToken cancellationToken)
    {
        var label = await _labelService.GetByIdAsync(id, cancellationToken);
        if (label is null)
        {
            return NotFound();
        }

        return Ok(label);
    }

    [HttpPost]
    public async Task<ActionResult<Label>> Create(Label label, CancellationToken cancellationToken)
    {
        var created = await _labelService.CreateAsync(label, cancellationToken);
        return CreatedAtAction(nameof(GetLabel), new { id = created.Id }, created);
    }
}
