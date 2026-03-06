namespace Shared.Data;

public class MemoryDatabase
{
    public List<Dictionary<string, object>> Users { get; set; } = new();
    public List<Dictionary<string, object>> Movies { get; set; } = new();
    public List<Dictionary<string, object>> Actors { get; set; } = new();
    public List<Dictionary<string, object>> ActorsMovies { get; set; } = new();
    
    private int nextUserId = 1;
    private int nextMovieId = 1;
    private int nextActorId = 1;
    private int nextActorMovieId = 1;

    public MemoryDatabase()
    {
        SeedData();
    }

    public int GetNextUserId() => nextUserId++;
    public int GetNextMovieId() => nextMovieId++;
    public int GetNextActorId() => nextActorId++;
    public int GetNextActorMovieId() => nextActorMovieId++;

    private void SeedData()
    {
        // Seed Users
        Users.Add(new Dictionary<string, object>
        {
            ["Id"] = nextUserId++,
            ["Username"] = "admin",
            ["Email"] = "admin@smdb.com",
            ["Role"] = "Admin"
        });

        Users.Add(new Dictionary<string, object>
        {
            ["Id"] = nextUserId++,
            ["Username"] = "user1",
            ["Email"] = "user1@smdb.com",
            ["Role"] = "User"
        });

        // Seed Movies
        Movies.Add(new Dictionary<string, object>
        {
            ["Id"] = nextMovieId++,
            ["Title"] = "The Shawshank Redemption",
            ["Year"] = 1994,
            ["Description"] = "Two imprisoned men bond over a number of years."
        });

        Movies.Add(new Dictionary<string, object>
        {
            ["Id"] = nextMovieId++,
            ["Title"] = "The Godfather",
            ["Year"] = 1972,
            ["Description"] = "The aging patriarch of an organized crime dynasty transfers control."
        });

        Movies.Add(new Dictionary<string, object>
        {
            ["Id"] = nextMovieId++,
            ["Title"] = "The Dark Knight",
            ["Year"] = 2008,
            ["Description"] = "Batman faces the Joker in Gotham City."
        });

        // Seed Actors
        Actors.Add(new Dictionary<string, object>
        {
            ["Id"] = nextActorId++,
            ["Name"] = "Morgan Freeman",
            ["BirthYear"] = 1937,
            ["Nationality"] = "American"
        });

        Actors.Add(new Dictionary<string, object>
        {
            ["Id"] = nextActorId++,
            ["Name"] = "Marlon Brando",
            ["BirthYear"] = 1924,
            ["Nationality"] = "American"
        });

        Actors.Add(new Dictionary<string, object>
        {
            ["Id"] = nextActorId++,
            ["Name"] = "Christian Bale",
            ["BirthYear"] = 1974,
            ["Nationality"] = "British"
        });

        // Seed ActorsMovies
        ActorsMovies.Add(new Dictionary<string, object>
        {
            ["Id"] = nextActorMovieId++,
            ["ActorId"] = 1,
            ["MovieId"] = 1,
            ["Role"] = "Ellis Boyd 'Red' Redding"
        });

        ActorsMovies.Add(new Dictionary<string, object>
        {
            ["Id"] = nextActorMovieId++,
            ["ActorId"] = 2,
            ["MovieId"] = 2,
            ["Role"] = "Don Vito Corleone"
        });

        ActorsMovies.Add(new Dictionary<string, object>
        {
            ["Id"] = nextActorMovieId++,
            ["ActorId"] = 3,
            ["MovieId"] = 3,
            ["Role"] = "Bruce Wayne / Batman"
        });
    }
}
