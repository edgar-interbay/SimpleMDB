namespace Shared.Http;

using System.Net;
using System.Collections;
using System.Collections.Specialized;
using System.Text.RegularExpressions;

public delegate Task HttpMiddleware(HttpListenerRequest req, HttpListenerResponse res, Hashtable props, Func<Task> next);

public class HttpRouter
{
    private List<(string method, string pattern, List<HttpMiddleware> middlewares)> routes = new();
    private bool useParametrizedMatching = false;

    public void UseParametrizedRouteMatching()
    {
        useParametrizedMatching = true;
    }

    public void Use(params HttpMiddleware[] middlewares)
    {
        routes.Add(("*", "*", middlewares.ToList()));
    }

    public void UseRouter(string basePath, HttpRouter subRouter)
    {
        routes.Add(("ROUTER", basePath, new List<HttpMiddleware> { async (req, res, props, next) =>
        {
            string path = props["req.path"]?.ToString() ?? "/";
            if (path.StartsWith(basePath) || path == basePath.TrimEnd('/'))
            {
                string originalPath = path;
                string newPath = path.Substring(basePath.Length);
                if (string.IsNullOrEmpty(newPath) || !newPath.StartsWith("/"))
                    newPath = "/" + newPath.TrimStart('/');
                props["req.path"] = newPath;
                
                await subRouter.Handle(req, res, props);
                props["req.path"] = originalPath;
            }
            await next();
        }}));
    }

    public void MapGet(string pattern, params HttpMiddleware[] middlewares)
    {
        routes.Add(("GET", pattern, middlewares.ToList()));
    }

    public void MapPost(string pattern, params HttpMiddleware[] middlewares)
    {
        routes.Add(("POST", pattern, middlewares.ToList()));
    }

    public void MapPut(string pattern, params HttpMiddleware[] middlewares)
    {
        routes.Add(("PUT", pattern, middlewares.ToList()));
    }

    public void MapDelete(string pattern, params HttpMiddleware[] middlewares)
    {
        routes.Add(("DELETE", pattern, middlewares.ToList()));
    }

    public async Task Handle(HttpListenerRequest req, HttpListenerResponse res, Hashtable props)
    {
        string path = props["req.path"]?.ToString() ?? req.Url?.AbsolutePath ?? "/";
        
        foreach (var (method, pattern, middlewares) in routes)
        {
            if (method == "*" || method == req.HttpMethod)
            {
                if (pattern == "*" || MatchRoute(pattern, path, props))
                {
                    await ExecuteMiddlewares(middlewares, req, res, props);
                    if ((bool)(props["res.sent"] ?? false))
                        return;
                }
            }
            else if (method == "ROUTER")
            {
                await ExecuteMiddlewares(middlewares, req, res, props);
                if ((bool)(props["res.sent"] ?? false))
                    return;
            }
        }
    }

    private bool MatchRoute(string pattern, string path, Hashtable props)
    {
        if (pattern == path) return true;
        
        if (useParametrizedMatching && pattern.Contains(":"))
        {
            var paramNames = new List<string>();
            var regexPattern = "^" + Regex.Replace(pattern, @":([^/]+)", match =>
            {
                paramNames.Add(match.Groups[1].Value);
                return @"([^/]+)";
            }) + "$";

            var match = Regex.Match(path, regexPattern);
            if (match.Success)
            {
                var urlParams = new NameValueCollection();
                for (int i = 0; i < paramNames.Count; i++)
                {
                    urlParams[paramNames[i]] = match.Groups[i + 1].Value;
                }
                props["req.params"] = urlParams;
                return true;
            }
        }
        
        return false;
    }

    private async Task ExecuteMiddlewares(List<HttpMiddleware> middlewares, HttpListenerRequest req, HttpListenerResponse res, Hashtable props)
    {
        int index = 0;
        
        async Task Next()
        {
            if (index < middlewares.Count)
            {
                var middleware = middlewares[index++];
                await middleware(req, res, props, Next);
            }
        }
        
        await Next();
    }
}
