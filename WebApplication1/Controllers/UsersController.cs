using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private static readonly List<User> _users = new()
    {
        new User { Id = 1, Name = "Alice Johnson", Email = "alice@example.com", Phone = "555-0101" },
        new User { Id = 2, Name = "Bob Smith", Email = "bob@example.com", Phone = "555-0102" }
    };

    private static int _nextId = 3;

    // GET: api/users
    [HttpGet]
    public ActionResult<IEnumerable<User>> GetAllUsers()
    {
        return Ok(_users);
    }

    // GET: api/users/1
    [HttpGet("{id}")]
    public ActionResult<User> GetUser(int id)
    {
        var user = _users.FirstOrDefault(u => u.Id == id);
        if (user == null)
        {
            return NotFound(new { error = $"User with ID {id} not found." });
        }
        return Ok(user);
    }

    // POST: api/users
    [HttpPost]
    public ActionResult<User> CreateUser([FromBody] User user)
    {
        // Validation is handled by [ApiController] + data annotations on the model
        if (_users.Any(u => u.Email == user.Email))
        {
            return Conflict(new { error = "A user with this email already exists." });
        }

        user.Id = _nextId++;
        _users.Add(user);
        return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
    }

    // PUT: api/users/1
    [HttpPut("{id}")]
    public ActionResult<User> UpdateUser(int id, [FromBody] User updatedUser)
    {
        var user = _users.FirstOrDefault(u => u.Id == id);
        if (user == null)
        {
            return NotFound(new { error = $"User with ID {id} not found." });
        }

        if (_users.Any(u => u.Email == updatedUser.Email && u.Id != id))
        {
            return Conflict(new { error = "A user with this email already exists." });
        }

        user.Name = updatedUser.Name;
        user.Email = updatedUser.Email;
        user.Phone = updatedUser.Phone;
        return Ok(user);
    }

    // DELETE: api/users/1
    [HttpDelete("{id}")]
    public ActionResult DeleteUser(int id)
    {
        var user = _users.FirstOrDefault(u => u.Id == id);
        if (user == null)
        {
            return NotFound(new { error = $"User with ID {id} not found." });
        }

        _users.Remove(user);
        return NoContent();
    }
}
