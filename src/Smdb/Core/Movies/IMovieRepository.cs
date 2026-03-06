namespace Smdb.Core.Movies;

using Shared.Http;

public interface IMovieRepository
{
    Task<PagedResult<Movie>> ReadMovies(int page, int size);
    Task<Movie?> CreateMovie(Movie movie);
    Task<Movie?> ReadMovie(int id);
    Task<Movie?> UpdateMovie(int id, Movie newData);
    Task<Movie?> DeleteMovie(int id);
    Task<PagedResult<Movie>> ReadMoviesByActorId(int actorId, int page, int size);
}
