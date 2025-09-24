using Keep.Domain.Entities;
using Keep.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Keep.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RemindersController : ControllerBase
{
    private readonly KeepDbContext _db;

    public RemindersController(KeepDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Reminder>>> GetReminders()
    {
        var reminders = await _db.Reminders
            .AsNoTracking()
            .ToListAsync();

        return Ok(reminders);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<Reminder>> GetReminder(Guid id)
    {
        var reminder = await _db.Reminders
            .AsNoTracking()
            .FirstOrDefaultAsync(r => r.Id == id);

        if (reminder is null)
        {
            return NotFound();
        }

        return Ok(reminder);
    }

    [HttpPost]
    public async Task<ActionResult<Reminder>> Create(Reminder reminder)
    {
        reminder.Id = Guid.NewGuid();

        _db.Reminders.Add(reminder);
        await _db.SaveChangesAsync();

        return CreatedAtAction(nameof(GetReminder), new { id = reminder.Id }, reminder);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, Reminder updated)
    {
        if (id != updated.Id)
        {
            return BadRequest();
        }

        var reminder = await _db.Reminders.FirstOrDefaultAsync(r => r.Id == id);
        if (reminder is null)
        {
            return NotFound();
        }

        reminder.RemindAt = updated.RemindAt;
        reminder.IsDone = updated.IsDone;

        await _db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var reminder = await _db.Reminders.FirstOrDefaultAsync(r => r.Id == id);
        if (reminder is null)
        {
            return NotFound();
        }

        _db.Reminders.Remove(reminder);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}
