namespace Smdb.Core.Users;

using Shared.Data;
using Shared.Http;

public class MemoryUserRepository : IUserRepository
{
    private readonly MemoryDatabase db;

    public MemoryUserRepository(MemoryDatabase db)
    {
        this.db = db;
    }

    public Task<PagedResult<User>> ReadUsers(int page, int size)
    {
        var totalItems = db.Users.Count;
        var items = db.Users
            .Skip((page - 1) * size)
            .Take(size)
            .Select(DictToUser)
            .ToList();

        var result = new PagedResult<User>
        {
            Items = items,
            Page = page,
            Size = size,
            TotalItems = totalItems
        };

        return Task.FromResult(result);
    }

    public Task<User?> CreateUser(User user)
    {
        user.Id = db.GetNextUserId();
        db.Users.Add(UserToDict(user));
        return Task.FromResult<User?>(user);
    }

    public Task<User?> ReadUser(int id)
    {
        var userDict = db.Users.FirstOrDefault(u => (int)u["Id"] == id);
        return Task.FromResult(userDict != null ? DictToUser(userDict) : null);
    }

    public Task<User?> UpdateUser(int id, User newData)
    {
        var userDict = db.Users.FirstOrDefault(u => (int)u["Id"] == id);
        if (userDict == null) return Task.FromResult<User?>(null);

        userDict["Username"] = newData.Username;
        userDict["Email"] = newData.Email;
        userDict["Role"] = newData.Role;

        return Task.FromResult<User?>(DictToUser(userDict));
    }

    public Task<User?> DeleteUser(int id)
    {
        var userDict = db.Users.FirstOrDefault(u => (int)u["Id"] == id);
        if (userDict == null) return Task.FromResult<User?>(null);

        db.Users.Remove(userDict);
        return Task.FromResult<User?>(DictToUser(userDict));
    }

    private User DictToUser(Dictionary<string, object> dict)
    {
        return new User
        {
            Id = (int)dict["Id"],
            Username = (string)dict["Username"],
            Email = (string)dict["Email"],
            Role = (string)dict["Role"]
        };
    }

    private Dictionary<string, object> UserToDict(User user)
    {
        return new Dictionary<string, object>
        {
            ["Id"] = user.Id,
            ["Username"] = user.Username,
            ["Email"] = user.Email,
            ["Role"] = user.Role
        };
    }
}
