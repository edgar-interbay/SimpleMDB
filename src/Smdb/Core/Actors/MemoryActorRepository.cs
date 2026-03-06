namespace Smdb.Core.Actors;

using Shared.Data;
using Shared.Http;

public class MemoryActorRepository : IActorRepository
{
    private readonly MemoryDatabase db;

    public MemoryActorRepository(MemoryDatabase db)
    {
        this.db = db;
    }

    public Task<PagedResult<Actor>> ReadActors(int page, int size)
    {
        var totalItems = db.Actors.Count;
        var items = db.Actors
            .Skip((page - 1) * size)
            .Take(size)
            .Select(DictToActor)
            .ToList();

        var result = new PagedResult<Actor>
        {
            Items = items,
            Page = page,
            Size = size,
            TotalItems = totalItems
        };

        return Task.FromResult(result);
    }

    public Task<Actor?> CreateActor(Actor actor)
    {
        actor.Id = db.GetNextActorId();
        db.Actors.Add(ActorToDict(actor));
        return Task.FromResult<Actor?>(actor);
    }

    public Task<Actor?> ReadActor(int id)
    {
        var actorDict = db.Actors.FirstOrDefault(a => (int)a["Id"] == id);
        return Task.FromResult(actorDict != null ? DictToActor(actorDict) : null);
    }

    public Task<Actor?> UpdateActor(int id, Actor newData)
    {
        var actorDict = db.Actors.FirstOrDefault(a => (int)a["Id"] == id);
        if (actorDict == null) return Task.FromResult<Actor?>(null);

        actorDict["Name"] = newData.Name;
        actorDict["BirthYear"] = newData.BirthYear;
        actorDict["Nationality"] = newData.Nationality;

        return Task.FromResult<Actor?>(DictToActor(actorDict));
    }

    public Task<Actor?> DeleteActor(int id)
    {
        var actorDict = db.Actors.FirstOrDefault(a => (int)a["Id"] == id);
        if (actorDict == null) return Task.FromResult<Actor?>(null);

        db.Actors.Remove(actorDict);
        return Task.FromResult<Actor?>(DictToActor(actorDict));
    }

    public Task<PagedResult<Actor>> ReadActorsByMovieId(int movieId, int page, int size)
    {
        var actorIds = db.ActorsMovies
            .Where(am => (int)am["MovieId"] == movieId)
            .Select(am => (int)am["ActorId"])
            .ToList();

        var actors = db.Actors
            .Where(a => actorIds.Contains((int)a["Id"]))
            .ToList();

        var totalItems = actors.Count;
        var items = actors
            .Skip((page - 1) * size)
            .Take(size)
            .Select(DictToActor)
            .ToList();

        var result = new PagedResult<Actor>
        {
            Items = items,
            Page = page,
            Size = size,
            TotalItems = totalItems
        };

        return Task.FromResult(result);
    }

    private Actor DictToActor(Dictionary<string, object> dict)
    {
        return new Actor
        {
            Id = (int)dict["Id"],
            Name = (string)dict["Name"],
            BirthYear = (int)dict["BirthYear"],
            Nationality = (string)dict["Nationality"]
        };
    }

    private Dictionary<string, object> ActorToDict(Actor actor)
    {
        return new Dictionary<string, object>
        {
            ["Id"] = actor.Id,
            ["Name"] = actor.Name,
            ["BirthYear"] = actor.BirthYear,
            ["Nationality"] = actor.Nationality
        };
    }
}
