namespace Smdb.Api.Genres;

using Abcs.Http;

public class GenresRouter : HttpRouter
{
	public GenresRouter(GenresController genresController)
	{
		UseParametrizedRouteMatching();
		MapGet("/", genresController.ReadGenres);
		MapPost("/", HttpUtils.ReadRequestBodyAsText, genresController.CreateGenre);
		MapGet("/:id", genresController.ReadGenre);
		MapPut("/:id", HttpUtils.ReadRequestBodyAsText, genresController.UpdateGenre);
		MapDelete("/:id", genresController.DeleteGenre);
	}
}
