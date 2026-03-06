namespace Smdb.Api.Movies;

using Shared.Http;
using Smdb.Api.ActorsMovies;

public class MoviesRouter : HttpRouter
{
    public MoviesRouter(MoviesController moviesController, ActorsMoviesController actorsMoviesController)
    {
        UseParametrizedRouteMatching();
        
        MapGet("/", moviesController.ReadMovies);
        MapPost("/", HttpUtils.ReadRequestBodyAsText, moviesController.CreateMovie);
        MapGet("/:id", moviesController.ReadMovie);
        MapPut("/:id", HttpUtils.ReadRequestBodyAsText, moviesController.UpdateMovie);
        MapDelete("/:id", moviesController.DeleteMovie);
        MapGet("/:id/actors", actorsMoviesController.ReadActorsByMovie);
    }
}
