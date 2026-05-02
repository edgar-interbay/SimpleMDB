using Microsoft.AspNetCore.Mvc;
using SimpleMovieDB.Models;

namespace SimpleMovieDB.Controllers;

public class SsrController : Controller
{
    // ── Helpers ────────────────────────────────────────────────────────────────

    private bool IsLoggedIn  => HttpContext.Session.GetString("UserId") != null;
    private bool IsAdmin     => HttpContext.Session.GetString("Role") == "Admin";
    private string? Username => HttpContext.Session.GetString("Username");

    private IActionResult RequireLogin()
    {
        ViewBag.Error = "You must be logged in to access this page.";
        return View("Unauthorized");
    }

    private IActionResult RequireAdmin()
    {
        ViewBag.Error = "You do not have permission to perform this action.";
        return View("Forbidden");
    }

    // ── Landing ────────────────────────────────────────────────────────────────

    // GET /Ssr
    public IActionResult Index()
    {
        ViewBag.Username = Username;
        ViewBag.IsAdmin  = IsAdmin;
        return View();
    }

    // ── MOVIES ─────────────────────────────────────────────────────────────────

    // GET /Ssr/Movies
    public IActionResult Movies()
    {
        if (!IsLoggedIn) return RequireLogin();
        ViewBag.Username = Username;
        ViewBag.IsAdmin  = IsAdmin;
        ViewBag.Movies   = MovieStore.Movies;
        return View();
    }

    // GET /Ssr/MovieDetails/5
    public IActionResult MovieDetails(int id)
    {
        if (!IsLoggedIn) return RequireLogin();
        var movie = MovieStore.Movies.FirstOrDefault(m => m.Id == id);
        if (movie == null) return NotFound();
        ViewBag.Username = Username;
        ViewBag.IsAdmin  = IsAdmin;
        return View(movie);
    }

    // GET /Ssr/CreateMovie
    public IActionResult CreateMovie()
    {
        if (!IsLoggedIn) return RequireLogin();
        if (!IsAdmin)    return RequireAdmin();
        return View();
    }

    // POST /Ssr/CreateMovie
    [HttpPost]
    public IActionResult CreateMovie(MovieModel movie)
    {
        if (!IsLoggedIn) return RequireLogin();
        if (!IsAdmin)    return RequireAdmin();

        if (string.IsNullOrWhiteSpace(movie.Title))
        { ViewBag.Error = "Title is required."; return View(movie); }
        if (movie.Year < 1888 || movie.Year > 9999)
        { ViewBag.Error = "Year must be between 1888 and 9999."; return View(movie); }

        movie.Id = MovieStore.NextId++;
        MovieStore.Movies.Add(movie);
        return RedirectToAction("Movies");
    }

    // GET /Ssr/EditMovie/5
    public IActionResult EditMovie(int id)
    {
        if (!IsLoggedIn) return RequireLogin();
        if (!IsAdmin)    return RequireAdmin();
        var movie = MovieStore.Movies.FirstOrDefault(m => m.Id == id);
        if (movie == null) return NotFound();
        return View(movie);
    }

    // POST /Ssr/EditMovie/5
    [HttpPost]
    public IActionResult EditMovie(int id, MovieModel updated)
    {
        if (!IsLoggedIn) return RequireLogin();
        if (!IsAdmin)    return RequireAdmin();

        var movie = MovieStore.Movies.FirstOrDefault(m => m.Id == id);
        if (movie == null) return NotFound();

        if (string.IsNullOrWhiteSpace(updated.Title))
        { ViewBag.Error = "Title is required."; return View(updated); }
        if (updated.Year < 1888 || updated.Year > 9999)
        { ViewBag.Error = "Year must be between 1888 and 9999."; return View(updated); }

        movie.Title       = updated.Title;
        movie.Year        = updated.Year;
        movie.Description = updated.Description;
        return RedirectToAction("Movies");
    }

    // POST /Ssr/DeleteMovie/5
    [HttpPost]
    public IActionResult DeleteMovie(int id)
    {
        if (!IsLoggedIn) return RequireLogin();
        if (!IsAdmin)    return RequireAdmin();
        var movie = MovieStore.Movies.FirstOrDefault(m => m.Id == id);
        if (movie == null) return NotFound();
        MovieStore.Movies.Remove(movie);
        return RedirectToAction("Movies");
    }

    // ── ACTORS ─────────────────────────────────────────────────────────────────

    // GET /Ssr/Actors
    public IActionResult Actors()
    {
        if (!IsLoggedIn) return RequireLogin();
        ViewBag.Username = Username;
        ViewBag.IsAdmin  = IsAdmin;
        ViewBag.Actors   = ActorStore.Actors;
        return View();
    }

    // GET /Ssr/ActorDetails/5
    public IActionResult ActorDetails(int id)
    {
        if (!IsLoggedIn) return RequireLogin();
        var actor = ActorStore.Actors.FirstOrDefault(a => a.Id == id);
        if (actor == null) return NotFound();
        ViewBag.Username = Username;
        ViewBag.IsAdmin  = IsAdmin;
        // Movies for this actor
        var links  = ActorMovieStore.Links.Where(l => l.ActorId == id).ToList();
        var movies = links.Select(l => MovieStore.Movies.FirstOrDefault(m => m.Id == l.MovieId))
                         .Where(m => m != null).ToList();
        ViewBag.Movies = movies;
        return View(actor);
    }

    // GET /Ssr/CreateActor
    public IActionResult CreateActor()
    {
        if (!IsLoggedIn) return RequireLogin();
        if (!IsAdmin)    return RequireAdmin();
        return View();
    }

    // POST /Ssr/CreateActor
    [HttpPost]
    public IActionResult CreateActor(ActorModel actor)
    {
        if (!IsLoggedIn) return RequireLogin();
        if (!IsAdmin)    return RequireAdmin();

        if (string.IsNullOrWhiteSpace(actor.FirstName))
        { ViewBag.Error = "First name is required."; return View(actor); }
        if (string.IsNullOrWhiteSpace(actor.LastName))
        { ViewBag.Error = "Last name is required."; return View(actor); }
        if (actor.Rating < 0 || actor.Rating > 5)
        { ViewBag.Error = "Rating must be between 0 and 5."; return View(actor); }

        actor.Id = ActorStore.NextId++;
        ActorStore.Actors.Add(actor);
        return RedirectToAction("Actors");
    }

    // GET /Ssr/EditActor/5
    public IActionResult EditActor(int id)
    {
        if (!IsLoggedIn) return RequireLogin();
        if (!IsAdmin)    return RequireAdmin();
        var actor = ActorStore.Actors.FirstOrDefault(a => a.Id == id);
        if (actor == null) return NotFound();
        return View(actor);
    }

    // POST /Ssr/EditActor/5
    [HttpPost]
    public IActionResult EditActor(int id, ActorModel updated)
    {
        if (!IsLoggedIn) return RequireLogin();
        if (!IsAdmin)    return RequireAdmin();

        var actor = ActorStore.Actors.FirstOrDefault(a => a.Id == id);
        if (actor == null) return NotFound();

        if (string.IsNullOrWhiteSpace(updated.FirstName))
        { ViewBag.Error = "First name is required."; return View(updated); }
        if (string.IsNullOrWhiteSpace(updated.LastName))
        { ViewBag.Error = "Last name is required."; return View(updated); }
        if (updated.Rating < 0 || updated.Rating > 5)
        { ViewBag.Error = "Rating must be between 0 and 5."; return View(updated); }

        actor.FirstName = updated.FirstName;
        actor.LastName  = updated.LastName;
        actor.Rating    = updated.Rating;
        return RedirectToAction("Actors");
    }

    // POST /Ssr/DeleteActor/5
    [HttpPost]
    public IActionResult DeleteActor(int id)
    {
        if (!IsLoggedIn) return RequireLogin();
        if (!IsAdmin)    return RequireAdmin();
        var actor = ActorStore.Actors.FirstOrDefault(a => a.Id == id);
        if (actor == null) return NotFound();
        ActorStore.Actors.Remove(actor);
        return RedirectToAction("Actors");
    }

    // ── USERS ──────────────────────────────────────────────────────────────────

    // GET /Ssr/Users
    public IActionResult Users()
    {
        if (!IsLoggedIn) return RequireLogin();
        if (!IsAdmin)    return RequireAdmin();
        ViewBag.Username = Username;
        ViewBag.IsAdmin  = IsAdmin;
        ViewBag.Users    = UserStore.Users;
        return View();
    }

    // GET /Ssr/UserDetails/5
    public IActionResult UserDetails(int id)
    {
        if (!IsLoggedIn) return RequireLogin();
        if (!IsAdmin)    return RequireAdmin();
        var user = UserStore.Users.FirstOrDefault(u => u.Id == id);
        if (user == null) return NotFound();
        ViewBag.Username = Username;
        ViewBag.IsAdmin  = IsAdmin;
        return View(user);
    }

    // GET /Ssr/EditUser/5
    public IActionResult EditUser(int id)
    {
        if (!IsLoggedIn) return RequireLogin();
        if (!IsAdmin)    return RequireAdmin();
        var user = UserStore.Users.FirstOrDefault(u => u.Id == id);
        if (user == null) return NotFound();
        return View(user);
    }

    // POST /Ssr/EditUser/5
    [HttpPost]
    public IActionResult EditUser(int id, UserModel updated)
    {
        if (!IsLoggedIn) return RequireLogin();
        if (!IsAdmin)    return RequireAdmin();

        var user = UserStore.Users.FirstOrDefault(u => u.Id == id);
        if (user == null) return NotFound();

        if (string.IsNullOrWhiteSpace(updated.Username))
        { ViewBag.Error = "Username is required."; return View(updated); }

        user.Username = updated.Username;
        user.Password = updated.Password;
        user.Role     = updated.Role;
        return RedirectToAction("Users");
    }

    // POST /Ssr/DeleteUser/5
    [HttpPost]
    public IActionResult DeleteUser(int id)
    {
        if (!IsLoggedIn) return RequireLogin();
        if (!IsAdmin)    return RequireAdmin();
        var user = UserStore.Users.FirstOrDefault(u => u.Id == id);
        if (user == null) return NotFound();
        UserStore.Users.Remove(user);
        return RedirectToAction("Users");
    }

    // ── ACTORS-MOVIES ──────────────────────────────────────────────────────────

    // GET /Ssr/MovieActors/5
    public IActionResult MovieActors(int id)
    {
        if (!IsLoggedIn) return RequireLogin();
        var movie = MovieStore.Movies.FirstOrDefault(m => m.Id == id);
        if (movie == null) return NotFound();
        ViewBag.Username   = Username;
        ViewBag.IsAdmin    = IsAdmin;
        ViewBag.Movie      = movie;
        ViewBag.AllActors  = ActorStore.Actors;
        var links  = ActorMovieStore.Links.Where(l => l.MovieId == id).ToList();
        ViewBag.LinkedActors = links.Select(l => ActorStore.Actors.FirstOrDefault(a => a.Id == l.ActorId))
                                    .Where(a => a != null).ToList();
        return View();
    }

    // POST /Ssr/AddActorToMovie
    [HttpPost]
    public IActionResult AddActorToMovie(int movieId, int actorId)
    {
        if (!IsLoggedIn) return RequireLogin();
        if (!IsAdmin)    return RequireAdmin();
        if (!ActorMovieStore.Links.Any(l => l.MovieId == movieId && l.ActorId == actorId))
            ActorMovieStore.Links.Add(new MovieActorModel { MovieId = movieId, ActorId = actorId });
        return RedirectToAction("MovieActors", new { id = movieId });
    }

    // POST /Ssr/RemoveActorFromMovie
    [HttpPost]
    public IActionResult RemoveActorFromMovie(int movieId, int actorId)
    {
        if (!IsLoggedIn) return RequireLogin();
        if (!IsAdmin)    return RequireAdmin();
        var link = ActorMovieStore.Links.FirstOrDefault(l => l.MovieId == movieId && l.ActorId == actorId);
        if (link != null) ActorMovieStore.Links.Remove(link);
        return RedirectToAction("MovieActors", new { id = movieId });
    }
}
