namespace Smdb.Core.Actors;

using Shared.Http;
using System.Net;

public class DefaultActorService : IActorService
{
    private readonly IActorRepository actorRepository;

    public DefaultActorService(IActorRepository actorRepository)
    {
        this.actorRepository = actorRepository;
    }

    public async Task<Result<PagedResult<Actor>>> ReadActors(int page, int size)
    {
        if (page < 1)
        {
            return new Result<PagedResult<Actor>>(
                new Exception("Page must be >= 1."),
                (int)HttpStatusCode.BadRequest);
        }

        if (size < 1)
        {
            return new Result<PagedResult<Actor>>(
                new Exception("Page size must be >= 1."),
                (int)HttpStatusCode.BadRequest);
        }

        var pagedResult = await actorRepository.ReadActors(page, size);
        return new Result<PagedResult<Actor>>(pagedResult, (int)HttpStatusCode.OK);
    }

    public async Task<Result<Actor>> CreateActor(Actor newActor)
    {
        var validationResult = ValidateActor(newActor);
        if (validationResult != null)
        {
            return validationResult;
        }

        var actor = await actorRepository.CreateActor(newActor);
        var result = actor == null
            ? new Result<Actor>(
                new Exception($"Could not create actor {newActor}."),
                (int)HttpStatusCode.NotFound)
            : new Result<Actor>(actor, (int)HttpStatusCode.Created);

        return result;
    }

    public async Task<Result<Actor>> ReadActor(int id)
    {
        var actor = await actorRepository.ReadActor(id);
        var result = actor == null
            ? new Result<Actor>(
                new Exception($"Could not read actor with id {id}."),
                (int)HttpStatusCode.NotFound)
            : new Result<Actor>(actor, (int)HttpStatusCode.OK);

        return result;
    }

    public async Task<Result<Actor>> UpdateActor(int id, Actor newData)
    {
        var validationResult = ValidateActor(newData);
        if (validationResult != null)
        {
            return validationResult;
        }

        var actor = await actorRepository.UpdateActor(id, newData);
        var result = actor == null
            ? new Result<Actor>(
                new Exception($"Could not update actor with id {id}."),
                (int)HttpStatusCode.NotFound)
            : new Result<Actor>(actor, (int)HttpStatusCode.OK);

        return result;
    }

    public async Task<Result<Actor>> DeleteActor(int id)
    {
        var actor = await actorRepository.DeleteActor(id);
        var result = actor == null
            ? new Result<Actor>(
                new Exception($"Could not delete actor with id {id}."),
                (int)HttpStatusCode.NotFound)
            : new Result<Actor>(actor, (int)HttpStatusCode.OK);

        return result;
    }

    public async Task<Result<PagedResult<Actor>>> ReadActorsByMovieId(int movieId, int page, int size)
    {
        if (page < 1)
        {
            return new Result<PagedResult<Actor>>(
                new Exception("Page must be >= 1."),
                (int)HttpStatusCode.BadRequest);
        }

        if (size < 1)
        {
            return new Result<PagedResult<Actor>>(
                new Exception("Page size must be >= 1."),
                (int)HttpStatusCode.BadRequest);
        }

        var pagedResult = await actorRepository.ReadActorsByMovieId(movieId, page, size);
        return new Result<PagedResult<Actor>>(pagedResult, (int)HttpStatusCode.OK);
    }

    private static Result<Actor>? ValidateActor(Actor? actorData)
    {
        if (actorData is null)
        {
            return new Result<Actor>(
                new Exception("Actor payload is required."),
                (int)HttpStatusCode.BadRequest);
        }

        if (string.IsNullOrWhiteSpace(actorData.Name))
        {
            return new Result<Actor>(
                new Exception("Name is required and cannot be empty."),
                (int)HttpStatusCode.BadRequest);
        }

        if (actorData.Name.Length > 256)
        {
            return new Result<Actor>(
                new Exception("Name cannot be longer than 256 characters."),
                (int)HttpStatusCode.BadRequest);
        }

        if (actorData.BirthYear < 1800 || actorData.BirthYear > DateTime.UtcNow.Year)
        {
            return new Result<Actor>(
                new Exception($"Birth year must be between 1800 and {DateTime.UtcNow.Year}."),
                (int)HttpStatusCode.BadRequest);
        }

        return null;
    }
}
