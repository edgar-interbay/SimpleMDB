namespace Shared.Http;

public class Result<T>
{
    public T? Data { get; set; }
    public Exception? Error { get; set; }
    public int StatusCode { get; set; }
    public bool IsSuccess => Error == null;

    public Result(T data, int statusCode)
    {
        Data = data;
        StatusCode = statusCode;
    }

    public Result(Exception error, int statusCode)
    {
        Error = error;
        StatusCode = statusCode;
    }
}
