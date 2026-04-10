using Microsoft.AspNetCore.Mvc;
using SimpleMovieDB.Models;

namespace SimpleMovieDB.Controllers;

[ApiController]
[Route("api/v1/movies")]
public class MoviesApiController : ControllerBase
{
    private static List<MovieModel> _movies = new List<MovieModel>
    {
        new MovieModel { Id = 1, Title = "Toy Story", Year = 1995, Description = "A cowboy doll is threatened by a new spaceman figure." },
        new MovieModel { Id = 2, Title = "The Matrix", Year = 1999, Description = "A hacker discovers the world is a simulation." },
        new MovieModel { Id = 3, Title = "Inception", Year = 2010, Description = "A thief enters dreams to plant an idea." },
        new MovieModel { Id = 4, Title = "Interstellar", Year = 2014, Description = "Astronauts travel through a wormhole near Saturn." },
        new MovieModel { Id = 5, Title = "The Dark Knight", Year = 2008, Description = "Batman faces the Joker in Gotham City." },
        new MovieModel { Id = 6, Title = "Pulp Fiction", Year = 1994, Description = "Interconnected stories of crime in Los Angeles." },
        new MovieModel { Id = 7, Title = "Forrest Gump", Year = 1994, Description = "A slow-witted man witnesses key historical events." },
        new MovieModel { Id = 8, Title = "The Shawshank Redemption", Year = 1994, Description = "A banker is sentenced to life in Shawshank prison." },
        new MovieModel { Id = 9, Title = "Goodfellas", Year = 1990, Description = "The rise and fall of a mob associate." },
        new MovieModel { Id = 10, Title = "Fight Club", Year = 1999, Description = "An insomniac forms an underground fight club." }
    };
    private static int _nextId = 11;

    // GET /api/v1/movies?page=1&size=9
    [HttpGet]
    public IActionResult GetAll([FromQuery] int page = 1, [FromQuery] int size = 9)
    {
        if (page < 1) page = 1;
        if (size < 1) size = 1;
        if (size > 100) size = 100;

        var total = _movies.Count;
        var totalPages = (int)Math.Ceiling(total / (double)size);
        var data = _movies.Skip((page - 1) * size).Take(size).ToList();

        return Ok(new
        {
            data,
            meta = new { page, size, total, totalPages }
        });
    }

    // GET /api/v1/movies/{id}
    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var movie = _movies.FirstOrDefault(m => m.Id == id);
        if (movie == null) return NotFound(new { message = $"Movie {id} not found." });
        return Ok(movie);
    }

    // POST /api/v1/movies
    [HttpPost]
    public IActionResult Create([FromBody] MovieModel movie)
    {
        if (string.IsNullOrWhiteSpace(movie.Title))
            return BadRequest(new { message = "Title is required." });
        if (movie.Year < 1888 || movie.Year > 9999)
            return BadRequest(new { message = "Year must be between 1888 and 9999." });

        movie.Id = _nextId++;
        _movies.Add(movie);
        return CreatedAtAction(nameof(GetById), new { id = movie.Id }, movie);
    }

    // PUT /api/v1/movies/{id}
    [HttpPut("{id}")]
    public IActionResult Update(int id, [FromBody] MovieModel updated)
    {
        var movie = _movies.FirstOrDefault(m => m.Id == id);
        if (movie == null) return NotFound(new { message = $"Movie {id} not found." });

        if (string.IsNullOrWhiteSpace(updated.Title))
            return BadRequest(new { message = "Title is required." });
        if (updated.Year < 1888 || updated.Year > 9999)
            return BadRequest(new { message = "Year must be between 1888 and 9999." });

        movie.Title = updated.Title;
        movie.Year = updated.Year;
        movie.Description = updated.Description;
        return Ok(movie);
    }

    // DELETE /api/v1/movies/{id}
    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var movie = _movies.FirstOrDefault(m => m.Id == id);
        if (movie == null) return NotFound(new { message = $"Movie {id} not found." });
        _movies.Remove(movie);
        return NoContent();
    }

    // GET /api/v1/movies/{movieId}/actors
    [HttpGet("{movieId}/actors")]
    public IActionResult GetActors(int movieId)
    {
        if (!_movies.Any(m => m.Id == movieId))
            return NotFound(new { message = $"Movie {movieId} not found." });
        return Ok(ActorMovieStore.Links.Where(l => l.MovieId == movieId).ToList());
    }

    // POST /api/v1/movies/{movieId}/actors/{actorId}
    [HttpPost("{movieId}/actors/{actorId}")]
    public IActionResult AddActor(int movieId, int actorId)
    {
        if (!_movies.Any(m => m.Id == movieId))
            return NotFound(new { message = $"Movie {movieId} not found." });
        if (ActorMovieStore.Links.Any(l => l.MovieId == movieId && l.ActorId == actorId))
            return BadRequest(new { message = "Actor is already linked to this movie." });
        ActorMovieStore.Links.Add(new MovieActorModel { MovieId = movieId, ActorId = actorId });
        return Created("", new { movieId, actorId });
    }

    // DELETE /api/v1/movies/{movieId}/actors/{actorId}
    [HttpDelete("{movieId}/actors/{actorId}")]
    public IActionResult RemoveActor(int movieId, int actorId)
    {
        var link = ActorMovieStore.Links.FirstOrDefault(l => l.MovieId == movieId && l.ActorId == actorId);
        if (link == null) return NotFound(new { message = "Link not found." });
        ActorMovieStore.Links.Remove(link);
        return NoContent();
    }
}
