namespace Smdb.Api.ActorsMovies;

using Shared.Http;

public class ActorsMoviesRouter : HttpRouter
{
    public ActorsMoviesRouter(ActorsMoviesController actorsMoviesController)
    {
        UseParametrizedRouteMatching();
        
        MapGet("/", actorsMoviesController.ReadActorsMovies);
        MapPost("/", HttpUtils.ReadRequestBodyAsText, actorsMoviesController.CreateActorMovie);
        MapGet("/:id", actorsMoviesController.ReadActorMovie);
        MapPut("/:id", HttpUtils.ReadRequestBodyAsText, actorsMoviesController.UpdateActorMovie);
        MapDelete("/:id", actorsMoviesController.DeleteActorMovie);
    }
}
