using Microsoft.AspNetCore.Mvc;
using SimpleMovieDB.Models;

namespace SimpleMovieDB.Controllers;

[ApiController]
[Route("api/v1/users")]
public class UsersApiController : ControllerBase
{
    private static List<UserModel> _users = new List<UserModel>
    {
        new UserModel { Id = 1, Username = "admin",   Password = "admin123", Role = "Admin"   },
        new UserModel { Id = 2, Username = "john_doe", Password = "pass123",  Role = "Regular" }
    };
    private static int _nextId = 3;

    // GET /api/v1/users
    [HttpGet]
    public IActionResult GetAll() => Ok(_users);

    // GET /api/v1/users/{id}
    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var user = _users.FirstOrDefault(u => u.Id == id);
        if (user == null) return NotFound(new { message = $"User {id} not found." });
        return Ok(user);
    }

    // POST /api/v1/users
    [HttpPost]
    public IActionResult Create([FromBody] UserModel user)
    {
        if (string.IsNullOrWhiteSpace(user.Username))
            return BadRequest(new { message = "Username is required." });
        if (string.IsNullOrWhiteSpace(user.Password))
            return BadRequest(new { message = "Password is required." });
        if (_users.Any(u => u.Username == user.Username))
            return BadRequest(new { message = "Username already exists." });

        user.Id = _nextId++;
        if (string.IsNullOrWhiteSpace(user.Role)) user.Role = "Regular";
        _users.Add(user);
        return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
    }

    // PUT /api/v1/users/{id}
    [HttpPut("{id}")]
    public IActionResult Update(int id, [FromBody] UserModel updated)
    {
        var user = _users.FirstOrDefault(u => u.Id == id);
        if (user == null) return NotFound(new { message = $"User {id} not found." });

        if (string.IsNullOrWhiteSpace(updated.Username))
            return BadRequest(new { message = "Username is required." });
        if (string.IsNullOrWhiteSpace(updated.Password))
            return BadRequest(new { message = "Password is required." });

        user.Username = updated.Username;
        user.Password = updated.Password;
        user.Role     = updated.Role ?? "Regular";
        return Ok(user);
    }

    // DELETE /api/v1/users/{id}
    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var user = _users.FirstOrDefault(u => u.Id == id);
        if (user == null) return NotFound(new { message = $"User {id} not found." });
        _users.Remove(user);
        return NoContent();
    }
}
