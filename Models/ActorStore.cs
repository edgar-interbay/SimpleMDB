namespace SimpleMovieDB.Models;

public static class ActorStore
{
    public static List<ActorModel> Actors = new List<ActorModel>
    {
        new ActorModel { Id = 1, FirstName = "Tom",      LastName = "Hanks",      Rating = 5.0 },
        new ActorModel { Id = 2, FirstName = "Meryl",    LastName = "Streep",     Rating = 4.9 },
        new ActorModel { Id = 3, FirstName = "Leonardo", LastName = "DiCaprio",   Rating = 4.8 },
        new ActorModel { Id = 4, FirstName = "Cate",     LastName = "Blanchett",  Rating = 4.7 },
        new ActorModel { Id = 5, FirstName = "Denzel",   LastName = "Washington", Rating = 4.8 }
    };
    public static int NextId = 6;
}
