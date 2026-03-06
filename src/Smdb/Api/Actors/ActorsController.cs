namespace Smdb.Api.Actors;

using System.Net;
using System.Collections;
using System.Collections.Specialized;
using System.Text.Json;
using Shared.Http;
using Smdb.Core.Actors;
using Smdb.Core.Movies;

public class ActorsController
{
    private readonly IActorService actorService;
    private readonly IMovieService movieService;

    public ActorsController(IActorService actorService, IMovieService movieService)
    {
        this.actorService = actorService;
        this.movieService = movieService;
    }

    public async Task ReadActors(HttpListenerRequest req, HttpListenerResponse res, Hashtable props, Func<Task> next)
    {
        var queryParams = (NameValueCollection)props["req.query"]!;
        int page = int.TryParse(queryParams["page"], out int p) ? p : 1;
        int size = int.TryParse(queryParams["size"], out int s) ? s : 10;

        var result = await actorService.ReadActors(page, size);
        await HttpUtils.SendResultResponse(req, res, props, result);
        await next();
    }

    public async Task CreateActor(HttpListenerRequest req, HttpListenerResponse res, Hashtable props, Func<Task> next)
    {
        var json = props["req.text"]?.ToString() ?? "";
        var actor = JsonSerializer.Deserialize<Actor>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        if (actor == null)
        {
            var errorResult = new Result<Actor>(new Exception("Invalid actor data"), (int)HttpStatusCode.BadRequest);
            await HttpUtils.SendResultResponse(req, res, props, errorResult);
            await next();
            return;
        }

        var result = await actorService.CreateActor(actor);
        await HttpUtils.SendResultResponse(req, res, props, result);
        await next();
    }

    public async Task ReadActor(HttpListenerRequest req, HttpListenerResponse res, Hashtable props, Func<Task> next)
    {
        var uParams = (NameValueCollection)props["req.params"]!;
        int id = int.TryParse(uParams["id"]!, out int i) ? i : -1;

        var result = await actorService.ReadActor(id);
        await HttpUtils.SendResultResponse(req, res, props, result);
        await next();
    }

    public async Task UpdateActor(HttpListenerRequest req, HttpListenerResponse res, Hashtable props, Func<Task> next)
    {
        var uParams = (NameValueCollection)props["req.params"]!;
        int id = int.TryParse(uParams["id"]!, out int i) ? i : -1;

        var json = props["req.text"]?.ToString() ?? "";
        var actor = JsonSerializer.Deserialize<Actor>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        if (actor == null)
        {
            var errorResult = new Result<Actor>(new Exception("Invalid actor data"), (int)HttpStatusCode.BadRequest);
            await HttpUtils.SendResultResponse(req, res, props, errorResult);
            await next();
            return;
        }

        var result = await actorService.UpdateActor(id, actor);
        await HttpUtils.SendResultResponse(req, res, props, result);
        await next();
    }

    public async Task DeleteActor(HttpListenerRequest req, HttpListenerResponse res, Hashtable props, Func<Task> next)
    {
        var uParams = (NameValueCollection)props["req.params"]!;
        int id = int.TryParse(uParams["id"]!, out int i) ? i : -1;

        var result = await actorService.DeleteActor(id);
        await HttpUtils.SendResultResponse(req, res, props, result);
        await next();
    }

    public async Task ReadMoviesByActor(HttpListenerRequest req, HttpListenerResponse res, Hashtable props, Func<Task> next)
    {
        var uParams = (NameValueCollection)props["req.params"]!;
        int id = int.TryParse(uParams["id"]!, out int i) ? i : -1;

        var queryParams = (NameValueCollection)props["req.query"]!;
        int page = int.TryParse(queryParams["page"], out int p) ? p : 1;
        int size = int.TryParse(queryParams["size"], out int s) ? s : 10;

        var result = await movieService.ReadMoviesByActorId(id, page, size);
        await HttpUtils.SendResultResponse(req, res, props, result);
        await next();
    }
}
