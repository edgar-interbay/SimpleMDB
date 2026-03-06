namespace Smdb.Api.ActorsMovies;

using System.Net;
using System.Collections;
using System.Collections.Specialized;
using System.Text.Json;
using Shared.Http;
using Smdb.Core.ActorsMovies;
using Smdb.Core.Actors;
using Smdb.Core.Movies;

public class ActorsMoviesController
{
    private readonly IActorMovieService actorMovieService;
    private readonly IActorService actorService;
    private readonly IMovieService movieService;

    public ActorsMoviesController(IActorMovieService actorMovieService, IActorService actorService, IMovieService movieService)
    {
        this.actorMovieService = actorMovieService;
        this.actorService = actorService;
        this.movieService = movieService;
    }

    public async Task ReadActorsMovies(HttpListenerRequest req, HttpListenerResponse res, Hashtable props, Func<Task> next)
    {
        var queryParams = (NameValueCollection)props["req.query"]!;
        int page = int.TryParse(queryParams["page"], out int p) ? p : 1;
        int size = int.TryParse(queryParams["size"], out int s) ? s : 10;

        var result = await actorMovieService.ReadActorsMovies(page, size);
        await HttpUtils.SendResultResponse(req, res, props, result);
        await next();
    }

    public async Task CreateActorMovie(HttpListenerRequest req, HttpListenerResponse res, Hashtable props, Func<Task> next)
    {
        var json = props["req.text"]?.ToString() ?? "";
        var actorMovie = JsonSerializer.Deserialize<ActorMovie>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        if (actorMovie == null)
        {
            var errorResult = new Result<ActorMovie>(new Exception("Invalid actor-movie data"), (int)HttpStatusCode.BadRequest);
            await HttpUtils.SendResultResponse(req, res, props, errorResult);
            await next();
            return;
        }

        var result = await actorMovieService.CreateActorMovie(actorMovie);
        await HttpUtils.SendResultResponse(req, res, props, result);
        await next();
    }

    public async Task ReadActorMovie(HttpListenerRequest req, HttpListenerResponse res, Hashtable props, Func<Task> next)
    {
        var uParams = (NameValueCollection)props["req.params"]!;
        int id = int.TryParse(uParams["id"]!, out int i) ? i : -1;

        var result = await actorMovieService.ReadActorMovie(id);
        await HttpUtils.SendResultResponse(req, res, props, result);
        await next();
    }

    public async Task UpdateActorMovie(HttpListenerRequest req, HttpListenerResponse res, Hashtable props, Func<Task> next)
    {
        var uParams = (NameValueCollection)props["req.params"]!;
        int id = int.TryParse(uParams["id"]!, out int i) ? i : -1;

        var json = props["req.text"]?.ToString() ?? "";
        var actorMovie = JsonSerializer.Deserialize<ActorMovie>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        if (actorMovie == null)
        {
            var errorResult = new Result<ActorMovie>(new Exception("Invalid actor-movie data"), (int)HttpStatusCode.BadRequest);
            await HttpUtils.SendResultResponse(req, res, props, errorResult);
            await next();
            return;
        }

        var result = await actorMovieService.UpdateActorMovie(id, actorMovie);
        await HttpUtils.SendResultResponse(req, res, props, result);
        await next();
    }

    public async Task DeleteActorMovie(HttpListenerRequest req, HttpListenerResponse res, Hashtable props, Func<Task> next)
    {
        var uParams = (NameValueCollection)props["req.params"]!;
        int id = int.TryParse(uParams["id"]!, out int i) ? i : -1;

        var result = await actorMovieService.DeleteActorMovie(id);
        await HttpUtils.SendResultResponse(req, res, props, result);
        await next();
    }

    public async Task ReadActorsByMovie(HttpListenerRequest req, HttpListenerResponse res, Hashtable props, Func<Task> next)
    {
        var uParams = (NameValueCollection)props["req.params"]!;
        int id = int.TryParse(uParams["id"]!, out int i) ? i : -1;

        var queryParams = (NameValueCollection)props["req.query"]!;
        int page = int.TryParse(queryParams["page"], out int p) ? p : 1;
        int size = int.TryParse(queryParams["size"], out int s) ? s : 10;

        var result = await actorService.ReadActorsByMovieId(id, page, size);
        await HttpUtils.SendResultResponse(req, res, props, result);
        await next();
    }
}
