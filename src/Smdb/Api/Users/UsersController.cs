namespace Smdb.Api.Users;

using System.Net;
using System.Collections;
using System.Collections.Specialized;
using System.Text.Json;
using Shared.Http;
using Smdb.Core.Users;

public class UsersController
{
    private readonly IUserService userService;

    public UsersController(IUserService userService)
    {
        this.userService = userService;
    }

    public async Task ReadUsers(HttpListenerRequest req, HttpListenerResponse res, Hashtable props, Func<Task> next)
    {
        var queryParams = (NameValueCollection)props["req.query"]!;
        int page = int.TryParse(queryParams["page"], out int p) ? p : 1;
        int size = int.TryParse(queryParams["size"], out int s) ? s : 10;

        var result = await userService.ReadUsers(page, size);
        await HttpUtils.SendResultResponse(req, res, props, result);
        await next();
    }

    public async Task CreateUser(HttpListenerRequest req, HttpListenerResponse res, Hashtable props, Func<Task> next)
    {
        var json = props["req.text"]?.ToString() ?? "";
        var user = JsonSerializer.Deserialize<User>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        if (user == null)
        {
            var errorResult = new Result<User>(new Exception("Invalid user data"), (int)HttpStatusCode.BadRequest);
            await HttpUtils.SendResultResponse(req, res, props, errorResult);
            await next();
            return;
        }

        var result = await userService.CreateUser(user);
        await HttpUtils.SendResultResponse(req, res, props, result);
        await next();
    }

    public async Task ReadUser(HttpListenerRequest req, HttpListenerResponse res, Hashtable props, Func<Task> next)
    {
        var uParams = (NameValueCollection)props["req.params"]!;
        int id = int.TryParse(uParams["id"]!, out int i) ? i : -1;

        var result = await userService.ReadUser(id);
        await HttpUtils.SendResultResponse(req, res, props, result);
        await next();
    }

    public async Task UpdateUser(HttpListenerRequest req, HttpListenerResponse res, Hashtable props, Func<Task> next)
    {
        var uParams = (NameValueCollection)props["req.params"]!;
        int id = int.TryParse(uParams["id"]!, out int i) ? i : -1;

        var json = props["req.text"]?.ToString() ?? "";
        var user = JsonSerializer.Deserialize<User>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        if (user == null)
        {
            var errorResult = new Result<User>(new Exception("Invalid user data"), (int)HttpStatusCode.BadRequest);
            await HttpUtils.SendResultResponse(req, res, props, errorResult);
            await next();
            return;
        }

        var result = await userService.UpdateUser(id, user);
        await HttpUtils.SendResultResponse(req, res, props, result);
        await next();
    }

    public async Task DeleteUser(HttpListenerRequest req, HttpListenerResponse res, Hashtable props, Func<Task> next)
    {
        var uParams = (NameValueCollection)props["req.params"]!;
        int id = int.TryParse(uParams["id"]!, out int i) ? i : -1;

        var result = await userService.DeleteUser(id);
        await HttpUtils.SendResultResponse(req, res, props, result);
        await next();
    }
}
