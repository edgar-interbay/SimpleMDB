namespace SimpleMovieDB.Models;

public class ActorModel 
{
    public int Id { get; set; }
    public string FirstName { get; set; } = "";
    public string LastName { get; set; } = "";
    public double Rating { get; set; }
}