using Microsoft.AspNetCore.Mvc;
using SimpleMovieDB.Models;

namespace SimpleMovieDB.Controllers;

[ApiController]
[Route("api/v1")]
public class ActorsMoviesApiController : ControllerBase
{
    private static List<MovieActorModel> _links = new List<MovieActorModel>
    {
        new MovieActorModel { MovieId = 1, ActorId = 1 },
        new MovieActorModel { MovieId = 2, ActorId = 3 }
    };

    // GET /api/v1/movies/{movieId}/actors
    [HttpGet("movies/{movieId}/actors")]
    public IActionResult GetActorsByMovie(int movieId) =>
        Ok(_links.Where(l => l.MovieId == movieId).ToList());

    // GET /api/v1/actors/{actorId}/movies
    [HttpGet("actors/{actorId}/movies")]
    public IActionResult GetMoviesByActor(int actorId) =>
        Ok(_links.Where(l => l.ActorId == actorId).ToList());

    // POST /api/v1/movies/{movieId}/actors/{actorId}
    [HttpPost("movies/{movieId}/actors/{actorId}")]
    public IActionResult AddActorToMovie(int movieId, int actorId)
    {
        if (_links.Any(l => l.MovieId == movieId && l.ActorId == actorId))
            return BadRequest(new { message = "Actor is already linked to this movie." });

        _links.Add(new MovieActorModel { MovieId = movieId, ActorId = actorId });
        return Created("", new { movieId, actorId });
    }

    // DELETE /api/v1/movies/{movieId}/actors/{actorId}
    [HttpDelete("movies/{movieId}/actors/{actorId}")]
    public IActionResult RemoveActorFromMovie(int movieId, int actorId)
    {
        var link = _links.FirstOrDefault(l => l.MovieId == movieId && l.ActorId == actorId);
        if (link == null) return NotFound(new { message = "Link not found." });
        _links.Remove(link);
        return NoContent();
    }
}
