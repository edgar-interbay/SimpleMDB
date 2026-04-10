using Microsoft.AspNetCore.Mvc;
using SimpleMovieDB.Models;

namespace SimpleMovieDB.Controllers;

// Shared data store used by MoviesApiController for actor-movie links
public static class ActorMovieStore
{
    public static List<MovieActorModel> Links = new List<MovieActorModel>
    {
        new MovieActorModel { MovieId = 1, ActorId = 1 },
        new MovieActorModel { MovieId = 2, ActorId = 3 }
    };
}
