namespace Smdb.Core.Movies;

public class Movie
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public int Year { get; set; }
    public string Description { get; set; } = string.Empty;
}
