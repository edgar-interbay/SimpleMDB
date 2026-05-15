namespace Smdb.Api.Genres;

using System.Collections;
using System.Collections.Specialized;
using System.Net;
using System.Text.Json;
using Abcs.Http;
using Smdb.Core.Genres;

public class GenresController
{
	private IGenreService genreService;

	public GenresController(IGenreService genreService)
	{
		this.genreService = genreService;
	}

	// curl -X GET "http://localhost:8080/api/v1/genres?page=1&size=10"
	public async Task ReadGenres(HttpListenerRequest req, HttpListenerResponse res, Hashtable props, Func<Task> next)
	{
		int page = int.TryParse(req.QueryString["page"], out int p) ? p : 1;
		int size = int.TryParse(req.QueryString["size"], out int s) ? s : int.MaxValue;

		var result = await genreService.ReadGenres(page, size);

		await JsonUtils.SendPagedResultResponse(req, res, props, result, page, size);
		
		await next();
	}

	// curl -X POST "http://localhost:8080/genres" -H "Content-Type: application/json" -d "{ \"id\": -1, \"title\": \"Inception\", \"year\": 2010, \"genre\": \"Science Fiction\", \"description\": \"A skilled thief who enters dreams to steal secrets.\", \"rating\": 9 }"
	public async Task CreateGenre(HttpListenerRequest req, HttpListenerResponse res, Hashtable props, Func<Task> next)
	{
		var text = (string) props["req.text"]!;
		var genre = JsonSerializer.Deserialize<Genre>(text, JsonUtils.DefaultOptions);
		var result = await genreService.CreateGenre(genre!);

		await JsonUtils.SendResultResponse(req, res, props, result);

		await next();
	}

	// curl -X GET "http://localhost:8080/genres/1"
	public async Task ReadGenre(HttpListenerRequest req, HttpListenerResponse res, Hashtable props, Func<Task> next)
	{
		var uParams = (NameValueCollection) props["urlParams"]!;
		int id = int.TryParse(uParams["id"]!, out int i) ? i : -1;

		var result = await genreService.ReadGenre(id);

		await JsonUtils.SendResultResponse(req, res, props, result);

		await next();
	}

	// curl -X PUT "http://localhost:8080/genres/1" -H "Content-Type: application/json" -d "{ \"title\": \"Joker 2\", \"year\": 2020, \"genre\": \"Crime\", \"description\": \"A man that is a joke.\", \"rating\": 7.9 }"
	public async Task UpdateGenre(HttpListenerRequest req, HttpListenerResponse res, Hashtable props, Func<Task> next)
	{
		var uParams = (NameValueCollection) props["urlParams"]!;
		int id = int.TryParse(uParams["id"]!, out int i) ? i : -1;
		var text = (string) props["req.text"]!;
		var genre = JsonSerializer.Deserialize<Genre>(text, JsonUtils.DefaultOptions);
		var result = await genreService.UpdateGenre(id, genre!);

		await JsonUtils.SendResultResponse(req, res, props, result);

		await next();
	}

	// curl -X DELETE http://localhost:8080/genres/1
	public async Task DeleteGenre(HttpListenerRequest req, HttpListenerResponse res, Hashtable props, Func<Task> next)
	{
		var uParams = (NameValueCollection) props["urlParams"]!;
		int id = int.TryParse(uParams["id"]!, out int i) ? i : -1;

		var result = await genreService.DeleteGenre(id);

		await JsonUtils.SendResultResponse(req, res, props, result);

		await next();
	}
}
