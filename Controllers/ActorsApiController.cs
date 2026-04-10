using Microsoft.AspNetCore.Mvc;
using SimpleMovieDB.Models;

namespace SimpleMovieDB.Controllers;

[ApiController]
[Route("api/v1/actors")]
public class ActorsApiController : ControllerBase
{
    private static List<ActorModel> _actors = new List<ActorModel>
    {
        new ActorModel { Id = 1, FirstName = "Tom",        LastName = "Hanks",      Rating = 5.0 },
        new ActorModel { Id = 2, FirstName = "Meryl",      LastName = "Streep",     Rating = 4.9 },
        new ActorModel { Id = 3, FirstName = "Leonardo",   LastName = "DiCaprio",   Rating = 4.8 },
        new ActorModel { Id = 4, FirstName = "Cate",       LastName = "Blanchett",  Rating = 4.7 },
        new ActorModel { Id = 5, FirstName = "Denzel",     LastName = "Washington", Rating = 4.8 }
    };
    private static int _nextId = 6;

    // GET /api/v1/actors
    [HttpGet]
    public IActionResult GetAll() => Ok(_actors);

    // GET /api/v1/actors/{id}
    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var actor = _actors.FirstOrDefault(a => a.Id == id);
        if (actor == null) return NotFound(new { message = $"Actor {id} not found." });
        return Ok(actor);
    }

    // POST /api/v1/actors
    [HttpPost]
    public IActionResult Create([FromBody] ActorModel actor)
    {
        if (string.IsNullOrWhiteSpace(actor.FirstName))
            return BadRequest(new { message = "First name is required." });
        if (string.IsNullOrWhiteSpace(actor.LastName))
            return BadRequest(new { message = "Last name is required." });
        if (actor.Rating < 0 || actor.Rating > 5)
            return BadRequest(new { message = "Rating must be between 0 and 5." });

        actor.Id = _nextId++;
        _actors.Add(actor);
        return CreatedAtAction(nameof(GetById), new { id = actor.Id }, actor);
    }

    // PUT /api/v1/actors/{id}
    [HttpPut("{id}")]
    public IActionResult Update(int id, [FromBody] ActorModel updated)
    {
        var actor = _actors.FirstOrDefault(a => a.Id == id);
        if (actor == null) return NotFound(new { message = $"Actor {id} not found." });

        if (string.IsNullOrWhiteSpace(updated.FirstName))
            return BadRequest(new { message = "First name is required." });
        if (string.IsNullOrWhiteSpace(updated.LastName))
            return BadRequest(new { message = "Last name is required." });
        if (updated.Rating < 0 || updated.Rating > 5)
            return BadRequest(new { message = "Rating must be between 0 and 5." });

        actor.FirstName = updated.FirstName;
        actor.LastName  = updated.LastName;
        actor.Rating    = updated.Rating;
        return Ok(actor);
    }

    // DELETE /api/v1/actors/{id}
    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var actor = _actors.FirstOrDefault(a => a.Id == id);
        if (actor == null) return NotFound(new { message = $"Actor {id} not found." });
        _actors.Remove(actor);
        return NoContent();
    }
}
