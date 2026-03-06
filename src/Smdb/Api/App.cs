namespace Smdb.Api;

using Shared.Http;
using Shared.Data;
using Smdb.Api.Movies;
using Smdb.Api.Actors;
using Smdb.Api.Users;
using Smdb.Api.ActorsMovies;
using Smdb.Core.Movies;
using Smdb.Core.Actors;
using Smdb.Core.Users;
using Smdb.Core.ActorsMovies;

public class App : HttpServer
{
    public override void Init()
    {
        // Initialize database
        var db = new MemoryDatabase();

        // Initialize repositories
        var movieRepo = new MemoryMovieRepository(db);
        var actorRepo = new MemoryActorRepository(db);
        var userRepo = new MemoryUserRepository(db);
        var actorMovieRepo = new MemoryActorMovieRepository(db);

        // Initialize services
        var movieServ = new DefaultMovieService(movieRepo);
        var actorServ = new DefaultActorService(actorRepo);
        var userServ = new DefaultUserService(userRepo);
        var actorMovieServ = new DefaultActorMovieService(actorMovieRepo);

        // Initialize controllers
        var movieCtrl = new MoviesController(movieServ);
        var actorCtrl = new ActorsController(actorServ, movieServ);
        var userCtrl = new UsersController(userServ);
        var actorMovieCtrl = new ActorsMoviesController(actorMovieServ, actorServ, movieServ);

        // Initialize routers
        var movieRouter = new MoviesRouter(movieCtrl, actorMovieCtrl);
        var actorRouter = new ActorsRouter(actorCtrl);
        var userRouter = new UsersRouter(userCtrl);
        var actorMovieRouter = new ActorsMoviesRouter(actorMovieCtrl);

        // Initialize API router
        var apiRouter = new HttpRouter();

        // Install global middleware
        router.Use(HttpUtils.StructuredLogging);
        router.Use(HttpUtils.CentralizedErrorHandling);
        router.Use(HttpUtils.AddResponseCorsHeaders);
        router.Use(HttpUtils.ParseRequestUrl);
        router.Use(HttpUtils.ParseRequestQueryString);
        router.UseParametrizedRouteMatching();

        // Install API routes
        router.UseRouter("/api/v1", apiRouter);
        apiRouter.UseParametrizedRouteMatching();
        apiRouter.UseRouter("/actors-movies", actorMovieRouter);
        apiRouter.UseRouter("/movies", movieRouter);
        apiRouter.UseRouter("/actors", actorRouter);
        apiRouter.UseRouter("/users", userRouter);
        
        // Install default response (must be last)
        router.Use(HttpUtils.DefaultResponse);
    }
}
