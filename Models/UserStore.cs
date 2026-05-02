namespace SimpleMovieDB.Models;

// Shared in-memory user store used by both Auth and SSR controllers
public static class UserStore
{
    public static List<UserModel> Users = new List<UserModel>
    {
        new UserModel { Id = 1, Username = "admin",    Password = "admin123", Role = "Admin"   },
        new UserModel { Id = 2, Username = "john_doe", Password = "pass123",  Role = "Regular" }
    };
    public static int NextId = 3;
}
