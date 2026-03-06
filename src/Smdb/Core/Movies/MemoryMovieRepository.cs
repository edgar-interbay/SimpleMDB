namespace Smdb.Core.Movies;

using Shared.Data;
using Shared.Http;

public class MemoryMovieRepository : IMovieRepository
{
    private readonly MemoryDatabase db;

    public MemoryMovieRepository(MemoryDatabase db)
    {
        this.db = db;
    }

    public Task<PagedResult<Movie>> ReadMovies(int page, int size)
    {
        var totalItems = db.Movies.Count;
        var items = db.Movies
            .Skip((page - 1) * size)
            .Take(size)
            .Select(DictToMovie)
            .ToList();

        var result = new PagedResult<Movie>
        {
            Items = items,
            Page = page,
            Size = size,
            TotalItems = totalItems
        };

        return Task.FromResult(result);
    }

    public Task<Movie?> CreateMovie(Movie movie)
    {
        movie.Id = db.GetNextMovieId();
        db.Movies.Add(MovieToDict(movie));
        return Task.FromResult<Movie?>(movie);
    }

    public Task<Movie?> ReadMovie(int id)
    {
        var movieDict = db.Movies.FirstOrDefault(m => (int)m["Id"] == id);
        return Task.FromResult(movieDict != null ? DictToMovie(movieDict) : null);
    }

    public Task<Movie?> UpdateMovie(int id, Movie newData)
    {
        var movieDict = db.Movies.FirstOrDefault(m => (int)m["Id"] == id);
        if (movieDict == null) return Task.FromResult<Movie?>(null);

        movieDict["Title"] = newData.Title;
        movieDict["Year"] = newData.Year;
        movieDict["Description"] = newData.Description;

        return Task.FromResult<Movie?>(DictToMovie(movieDict));
    }

    public Task<Movie?> DeleteMovie(int id)
    {
        var movieDict = db.Movies.FirstOrDefault(m => (int)m["Id"] == id);
        if (movieDict == null) return Task.FromResult<Movie?>(null);

        db.Movies.Remove(movieDict);
        return Task.FromResult<Movie?>(DictToMovie(movieDict));
    }

    public Task<PagedResult<Movie>> ReadMoviesByActorId(int actorId, int page, int size)
    {
        var movieIds = db.ActorsMovies
            .Where(am => (int)am["ActorId"] == actorId)
            .Select(am => (int)am["MovieId"])
            .ToList();

        var movies = db.Movies
            .Where(m => movieIds.Contains((int)m["Id"]))
            .ToList();

        var totalItems = movies.Count;
        var items = movies
            .Skip((page - 1) * size)
            .Take(size)
            .Select(DictToMovie)
            .ToList();

        var result = new PagedResult<Movie>
        {
            Items = items,
            Page = page,
            Size = size,
            TotalItems = totalItems
        };

        return Task.FromResult(result);
    }

    private Movie DictToMovie(Dictionary<string, object> dict)
    {
        return new Movie
        {
            Id = (int)dict["Id"],
            Title = (string)dict["Title"],
            Year = (int)dict["Year"],
            Description = (string)dict["Description"]
        };
    }

    private Dictionary<string, object> MovieToDict(Movie movie)
    {
        return new Dictionary<string, object>
        {
            ["Id"] = movie.Id,
            ["Title"] = movie.Title,
            ["Year"] = movie.Year,
            ["Description"] = movie.Description
        };
    }
}
