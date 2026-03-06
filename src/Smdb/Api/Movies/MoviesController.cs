namespace Smdb.Api.Movies;

using System.Net;
using System.Collections;
using System.Collections.Specialized;
using System.Text.Json;
using Shared.Http;
using Smdb.Core.Movies;

public class MoviesController
{
    private readonly IMovieService movieService;

    public MoviesController(IMovieService movieService)
    {
        this.movieService = movieService;
    }

    public async Task ReadMovies(HttpListenerRequest req, HttpListenerResponse res, Hashtable props, Func<Task> next)
    {
        var queryParams = (NameValueCollection)props["req.query"]!;
        int page = int.TryParse(queryParams["page"], out int p) ? p : 1;
        int size = int.TryParse(queryParams["size"], out int s) ? s : 10;

        var result = await movieService.ReadMovies(page, size);
        await HttpUtils.SendResultResponse(req, res, props, result);
        await next();
    }

    public async Task CreateMovie(HttpListenerRequest req, HttpListenerResponse res, Hashtable props, Func<Task> next)
    {
        var json = props["req.text"]?.ToString() ?? "";
        var movie = JsonSerializer.Deserialize<Movie>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        if (movie == null)
        {
            var errorResult = new Result<Movie>(new Exception("Invalid movie data"), (int)HttpStatusCode.BadRequest);
            await HttpUtils.SendResultResponse(req, res, props, errorResult);
            await next();
            return;
        }

        var result = await movieService.CreateMovie(movie);
        await HttpUtils.SendResultResponse(req, res, props, result);
        await next();
    }

    public async Task ReadMovie(HttpListenerRequest req, HttpListenerResponse res, Hashtable props, Func<Task> next)
    {
        var uParams = (NameValueCollection)props["req.params"]!;
        int id = int.TryParse(uParams["id"]!, out int i) ? i : -1;

        var result = await movieService.ReadMovie(id);
        await HttpUtils.SendResultResponse(req, res, props, result);
        await next();
    }

    public async Task UpdateMovie(HttpListenerRequest req, HttpListenerResponse res, Hashtable props, Func<Task> next)
    {
        var uParams = (NameValueCollection)props["req.params"]!;
        int id = int.TryParse(uParams["id"]!, out int i) ? i : -1;

        var json = props["req.text"]?.ToString() ?? "";
        var movie = JsonSerializer.Deserialize<Movie>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        if (movie == null)
        {
            var errorResult = new Result<Movie>(new Exception("Invalid movie data"), (int)HttpStatusCode.BadRequest);
            await HttpUtils.SendResultResponse(req, res, props, errorResult);
            await next();
            return;
        }

        var result = await movieService.UpdateMovie(id, movie);
        await HttpUtils.SendResultResponse(req, res, props, result);
        await next();
    }

    public async Task DeleteMovie(HttpListenerRequest req, HttpListenerResponse res, Hashtable props, Func<Task> next)
    {
        var uParams = (NameValueCollection)props["req.params"]!;
        int id = int.TryParse(uParams["id"]!, out int i) ? i : -1;

        var result = await movieService.DeleteMovie(id);
        await HttpUtils.SendResultResponse(req, res, props, result);
        await next();
    }
}
