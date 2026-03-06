namespace Shared.Http;

using System.Net;
using System.Collections;

public abstract class HttpServer
{
    protected HttpRouter router = new HttpRouter();
    private HttpListener? listener;
    private bool isRunning = false;

    public abstract void Init();

    public async Task Start()
    {
        var config = LoadConfig("appsettings.cfg");
        string host = config.GetValueOrDefault("HOST", "http://localhost");
        string port = config.GetValueOrDefault("PORT", "3000");
        string url = $"{host}:{port}/";

        Init();

        listener = new HttpListener();
        listener.Prefixes.Add(url);
        listener.Start();
        isRunning = true;

        Console.WriteLine($"Server running at {url}");
        Console.WriteLine("Press Ctrl+C to stop");

        while (isRunning)
        {
            try
            {
                var context = await listener.GetContextAsync();
                _ = Task.Run(async () => await HandleRequest(context));
            }
            catch (HttpListenerException)
            {
                break;
            }
        }
    }

    private async Task HandleRequest(HttpListenerContext context)
    {
        var req = context.Request;
        var res = context.Response;
        var props = new Hashtable();

        try
        {
            await router.Handle(req, res, props);
        }
        finally
        {
            res.Close();
        }
    }

    private Dictionary<string, string> LoadConfig(string path)
    {
        var config = new Dictionary<string, string>();
        
        if (File.Exists(path))
        {
            foreach (var line in File.ReadAllLines(path))
            {
                if (string.IsNullOrWhiteSpace(line) || line.StartsWith("#"))
                    continue;
                    
                var parts = line.Split('=', 2);
                if (parts.Length == 2)
                {
                    config[parts[0].Trim()] = parts[1].Trim();
                }
            }
        }
        
        return config;
    }

    public void Stop()
    {
        isRunning = false;
        listener?.Stop();
    }
}
