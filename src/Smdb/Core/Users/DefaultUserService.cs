namespace Smdb.Core.Users;

using Shared.Http;
using System.Net;

public class DefaultUserService : IUserService
{
    private readonly IUserRepository userRepository;

    public DefaultUserService(IUserRepository userRepository)
    {
        this.userRepository = userRepository;
    }

    public async Task<Result<PagedResult<User>>> ReadUsers(int page, int size)
    {
        if (page < 1)
        {
            return new Result<PagedResult<User>>(
                new Exception("Page must be >= 1."),
                (int)HttpStatusCode.BadRequest);
        }

        if (size < 1)
        {
            return new Result<PagedResult<User>>(
                new Exception("Page size must be >= 1."),
                (int)HttpStatusCode.BadRequest);
        }

        var pagedResult = await userRepository.ReadUsers(page, size);
        return new Result<PagedResult<User>>(pagedResult, (int)HttpStatusCode.OK);
    }

    public async Task<Result<User>> CreateUser(User newUser)
    {
        var validationResult = ValidateUser(newUser);
        if (validationResult != null)
        {
            return validationResult;
        }

        var user = await userRepository.CreateUser(newUser);
        var result = user == null
            ? new Result<User>(
                new Exception($"Could not create user {newUser}."),
                (int)HttpStatusCode.NotFound)
            : new Result<User>(user, (int)HttpStatusCode.Created);

        return result;
    }

    public async Task<Result<User>> ReadUser(int id)
    {
        var user = await userRepository.ReadUser(id);
        var result = user == null
            ? new Result<User>(
                new Exception($"Could not read user with id {id}."),
                (int)HttpStatusCode.NotFound)
            : new Result<User>(user, (int)HttpStatusCode.OK);

        return result;
    }

    public async Task<Result<User>> UpdateUser(int id, User newData)
    {
        var validationResult = ValidateUser(newData);
        if (validationResult != null)
        {
            return validationResult;
        }

        var user = await userRepository.UpdateUser(id, newData);
        var result = user == null
            ? new Result<User>(
                new Exception($"Could not update user with id {id}."),
                (int)HttpStatusCode.NotFound)
            : new Result<User>(user, (int)HttpStatusCode.OK);

        return result;
    }

    public async Task<Result<User>> DeleteUser(int id)
    {
        var user = await userRepository.DeleteUser(id);
        var result = user == null
            ? new Result<User>(
                new Exception($"Could not delete user with id {id}."),
                (int)HttpStatusCode.NotFound)
            : new Result<User>(user, (int)HttpStatusCode.OK);

        return result;
    }

    private static Result<User>? ValidateUser(User? userData)
    {
        if (userData is null)
        {
            return new Result<User>(
                new Exception("User payload is required."),
                (int)HttpStatusCode.BadRequest);
        }

        if (string.IsNullOrWhiteSpace(userData.Username))
        {
            return new Result<User>(
                new Exception("Username is required and cannot be empty."),
                (int)HttpStatusCode.BadRequest);
        }

        if (userData.Username.Length > 50)
        {
            return new Result<User>(
                new Exception("Username cannot be longer than 50 characters."),
                (int)HttpStatusCode.BadRequest);
        }

        if (string.IsNullOrWhiteSpace(userData.Email))
        {
            return new Result<User>(
                new Exception("Email is required and cannot be empty."),
                (int)HttpStatusCode.BadRequest);
        }

        if (!userData.Email.Contains("@"))
        {
            return new Result<User>(
                new Exception("Email must be valid."),
                (int)HttpStatusCode.BadRequest);
        }

        return null;
    }
}
