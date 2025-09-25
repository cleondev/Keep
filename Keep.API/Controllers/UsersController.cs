using Keep.Application.Users;
using Keep.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Keep.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<User>>> GetUsers(CancellationToken cancellationToken)
    {
        var users = await _userService.GetAllAsync(cancellationToken);
        return Ok(users);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<User>> GetUser(Guid id, CancellationToken cancellationToken)
    {
        var user = await _userService.GetByIdAsync(id, cancellationToken);
        if (user is null)
        {
            return NotFound();
        }

        return Ok(user);
    }

    [HttpPost]
    public async Task<ActionResult<User>> Create(User user, CancellationToken cancellationToken)
    {
        var created = await _userService.CreateAsync(user, cancellationToken);
        return CreatedAtAction(nameof(GetUser), new { id = created.Id }, created);
    }
}
