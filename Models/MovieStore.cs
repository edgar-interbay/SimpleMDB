namespace SimpleMovieDB.Models;

public static class MovieStore
{
    public static List<MovieModel> Movies = new List<MovieModel>
    {
        new MovieModel { Id = 1, Title = "Toy Story",                Year = 1995, Description = "A cowboy doll is threatened by a new spaceman figure." },
        new MovieModel { Id = 2, Title = "The Matrix",               Year = 1999, Description = "A hacker discovers the world is a simulation." },
        new MovieModel { Id = 3, Title = "Inception",                Year = 2010, Description = "A thief enters dreams to plant an idea." },
        new MovieModel { Id = 4, Title = "Interstellar",             Year = 2014, Description = "Astronauts travel through a wormhole near Saturn." },
        new MovieModel { Id = 5, Title = "The Dark Knight",          Year = 2008, Description = "Batman faces the Joker in Gotham City." },
        new MovieModel { Id = 6, Title = "Pulp Fiction",             Year = 1994, Description = "Interconnected stories of crime in Los Angeles." },
        new MovieModel { Id = 7, Title = "Forrest Gump",             Year = 1994, Description = "A slow-witted man witnesses key historical events." },
        new MovieModel { Id = 8, Title = "The Shawshank Redemption", Year = 1994, Description = "A banker is sentenced to life in Shawshank prison." },
        new MovieModel { Id = 9, Title = "Goodfellas",               Year = 1990, Description = "The rise and fall of a mob associate." },
        new MovieModel { Id = 10, Title = "Fight Club",              Year = 1999, Description = "An insomniac forms an underground fight club." }
    };
    public static int NextId = 11;
}
