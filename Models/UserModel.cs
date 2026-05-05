namespace SimpleMovieDB.Models;

public class UserModel 
{
    public int Id { get; set; }
    public string Username { get; set; } = "";
    public string Password { get; set; } = "";
    public string Role { get; set; } = "Regular"; // Puede ser Admin o Regular
}