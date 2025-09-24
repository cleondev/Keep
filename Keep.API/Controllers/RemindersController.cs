using Keep.Application.Reminders;
using Keep.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Keep.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RemindersController : ControllerBase
{
    private readonly IReminderService _reminderService;

    public RemindersController(IReminderService reminderService)
    {
        _reminderService = reminderService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Reminder>>> GetReminders(CancellationToken cancellationToken)
    {
        var reminders = await _reminderService.GetAllAsync(cancellationToken);
        return Ok(reminders);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<Reminder>> GetReminder(Guid id, CancellationToken cancellationToken)
    {
        var reminder = await _reminderService.GetByIdAsync(id, cancellationToken);
        if (reminder is null)
        {
            return NotFound();
        }

        return Ok(reminder);
    }

    [HttpPost]
    public async Task<ActionResult<Reminder>> Create(Reminder reminder, CancellationToken cancellationToken)
    {
        var created = await _reminderService.CreateAsync(reminder, cancellationToken);
        return CreatedAtAction(nameof(GetReminder), new { id = created.Id }, created);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, Reminder updated, CancellationToken cancellationToken)
    {
        if (id != updated.Id)
        {
            return BadRequest();
        }

        var success = await _reminderService.UpdateAsync(updated, cancellationToken);
        if (!success)
        {
            return NotFound();
        }

        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var success = await _reminderService.DeleteAsync(id, cancellationToken);
        if (!success)
        {
            return NotFound();
        }

        return NoContent();
    }
}
