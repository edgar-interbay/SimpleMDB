namespace Smdb.Core.ActorsMovies;

using Shared.Data;
using Shared.Http;

public class MemoryActorMovieRepository : IActorMovieRepository
{
    private readonly MemoryDatabase db;

    public MemoryActorMovieRepository(MemoryDatabase db)
    {
        this.db = db;
    }

    public Task<PagedResult<ActorMovie>> ReadActorsMovies(int page, int size)
    {
        var totalItems = db.ActorsMovies.Count;
        var items = db.ActorsMovies
            .Skip((page - 1) * size)
            .Take(size)
            .Select(DictToActorMovie)
            .ToList();

        var result = new PagedResult<ActorMovie>
        {
            Items = items,
            Page = page,
            Size = size,
            TotalItems = totalItems
        };

        return Task.FromResult(result);
    }

    public Task<ActorMovie?> CreateActorMovie(ActorMovie actorMovie)
    {
        actorMovie.Id = db.GetNextActorMovieId();
        db.ActorsMovies.Add(ActorMovieToDict(actorMovie));
        return Task.FromResult<ActorMovie?>(actorMovie);
    }

    public Task<ActorMovie?> ReadActorMovie(int id)
    {
        var amDict = db.ActorsMovies.FirstOrDefault(am => (int)am["Id"] == id);
        return Task.FromResult(amDict != null ? DictToActorMovie(amDict) : null);
    }

    public Task<ActorMovie?> UpdateActorMovie(int id, ActorMovie newData)
    {
        var amDict = db.ActorsMovies.FirstOrDefault(am => (int)am["Id"] == id);
        if (amDict == null) return Task.FromResult<ActorMovie?>(null);

        amDict["ActorId"] = newData.ActorId;
        amDict["MovieId"] = newData.MovieId;
        amDict["Role"] = newData.Role;

        return Task.FromResult<ActorMovie?>(DictToActorMovie(amDict));
    }

    public Task<ActorMovie?> DeleteActorMovie(int id)
    {
        var amDict = db.ActorsMovies.FirstOrDefault(am => (int)am["Id"] == id);
        if (amDict == null) return Task.FromResult<ActorMovie?>(null);

        db.ActorsMovies.Remove(amDict);
        return Task.FromResult<ActorMovie?>(DictToActorMovie(amDict));
    }

    private ActorMovie DictToActorMovie(Dictionary<string, object> dict)
    {
        return new ActorMovie
        {
            Id = (int)dict["Id"],
            ActorId = (int)dict["ActorId"],
            MovieId = (int)dict["MovieId"],
            Role = (string)dict["Role"]
        };
    }

    private Dictionary<string, object> ActorMovieToDict(ActorMovie am)
    {
        return new Dictionary<string, object>
        {
            ["Id"] = am.Id,
            ["ActorId"] = am.ActorId,
            ["MovieId"] = am.MovieId,
            ["Role"] = am.Role
        };
    }
}
