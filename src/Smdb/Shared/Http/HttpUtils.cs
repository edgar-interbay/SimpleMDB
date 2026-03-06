namespace Shared.Http;

using System.Net;
using System.Collections;
using System.Text;
using System.Text.Json;
using System.Web;

public static class HttpUtils
{
    public static async Task StructuredLogging(HttpListenerRequest req, HttpListenerResponse res, Hashtable props, Func<Task> next)
    {
        Console.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss}] {req.HttpMethod} {req.Url?.AbsolutePath}");
        await next();
    }

    public static async Task CentralizedErrorHandling(HttpListenerRequest req, HttpListenerResponse res, Hashtable props, Func<Task> next)
    {
        try
        {
            await next();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"ERROR: {ex.Message}");
            res.StatusCode = 500;
            var errorResponse = new { error = "Internal server error", message = ex.Message };
            var json = JsonSerializer.Serialize(errorResponse);
            var buffer = Encoding.UTF8.GetBytes(json);
            res.ContentType = "application/json";
            await res.OutputStream.WriteAsync(buffer);
            props["res.sent"] = true;
        }
    }

    public static async Task AddResponseCorsHeaders(HttpListenerRequest req, HttpListenerResponse res, Hashtable props, Func<Task> next)
    {
        res.Headers.Add("Access-Control-Allow-Origin", "*");
        res.Headers.Add("Access-Control-Allow-Methods", "GET, POST, PUT, DELETE, OPTIONS");
        res.Headers.Add("Access-Control-Allow-Headers", "Content-Type");
        
        if (req.HttpMethod == "OPTIONS")
        {
            res.StatusCode = 204;
            res.Close();
            props["res.sent"] = true;
            return;
        }
        
        await next();
    }

    public static async Task DefaultResponse(HttpListenerRequest req, HttpListenerResponse res, Hashtable props, Func<Task> next)
    {
        await next();
        
        if (!(bool)(props["res.sent"] ?? false))
        {
            res.StatusCode = 404;
            var errorResponse = new { error = "Not found" };
            var json = JsonSerializer.Serialize(errorResponse);
            var buffer = Encoding.UTF8.GetBytes(json);
            res.ContentType = "application/json";
            await res.OutputStream.WriteAsync(buffer);
            props["res.sent"] = true;
        }
    }

    public static async Task ParseRequestUrl(HttpListenerRequest req, HttpListenerResponse res, Hashtable props, Func<Task> next)
    {
        props["req.path"] = req.Url?.AbsolutePath ?? "/";
        await next();
    }

    public static async Task ParseRequestQueryString(HttpListenerRequest req, HttpListenerResponse res, Hashtable props, Func<Task> next)
    {
        props["req.query"] = HttpUtility.ParseQueryString(req.Url?.Query ?? "");
        await next();
    }

    public static async Task ReadRequestBodyAsText(HttpListenerRequest req, HttpListenerResponse res, Hashtable props, Func<Task> next)
    {
        using var reader = new StreamReader(req.InputStream, req.ContentEncoding);
        props["req.text"] = await reader.ReadToEndAsync();
        await next();
    }

    public static async Task SendResultResponse<T>(HttpListenerRequest req, HttpListenerResponse res, Hashtable props, Result<T> result)
    {
        res.StatusCode = result.StatusCode;
        res.ContentType = "application/json";

        object responseData;
        if (result.IsSuccess)
        {
            responseData = result.Data!;
        }
        else
        {
            responseData = new { error = result.Error?.Message };
        }

        var json = JsonSerializer.Serialize(responseData);
        var buffer = Encoding.UTF8.GetBytes(json);
        await res.OutputStream.WriteAsync(buffer);
        props["res.sent"] = true;
    }
}
