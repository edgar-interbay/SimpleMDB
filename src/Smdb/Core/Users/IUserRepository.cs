namespace Smdb.Core.Users;

using Shared.Http;

public interface IUserRepository
{
    Task<PagedResult<User>> ReadUsers(int page, int size);
    Task<User?> CreateUser(User user);
    Task<User?> ReadUser(int id);
    Task<User?> UpdateUser(int id, User newData);
    Task<User?> DeleteUser(int id);
}
