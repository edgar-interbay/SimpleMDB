namespace Smdb.Core.ActorsMovies;

using Shared.Http;

public interface IActorMovieService
{
    Task<Result<PagedResult<ActorMovie>>> ReadActorsMovies(int page, int size);
    Task<Result<ActorMovie>> CreateActorMovie(ActorMovie actorMovie);
    Task<Result<ActorMovie>> ReadActorMovie(int id);
    Task<Result<ActorMovie>> UpdateActorMovie(int id, ActorMovie newData);
    Task<Result<ActorMovie>> DeleteActorMovie(int id);
}
