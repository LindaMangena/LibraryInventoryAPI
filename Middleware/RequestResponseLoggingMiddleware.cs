using Microsoft.AspNetCore.Http;
using System.Diagnostics;
using System.Text.Json;
using System.Threading.Tasks;

public class RequestResponseLoggingMiddleware
{
    private readonly RequestDelegate _next;

    public RequestResponseLoggingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var stopwatch = new Stopwatch();
        stopwatch.Start();

        context.Response.OnStarting(state =>
        {
            var httpContext = (HttpContext)state;
            stopwatch.Stop();
            var logDetails = new
            {
                Method = httpContext.Request.Method,
                Url = httpContext.Request.Path,
                StatusCode = httpContext.Response.StatusCode,
                ResponseTime = stopwatch.ElapsedMilliseconds
            };

            Console.WriteLine(JsonSerializer.Serialize(logDetails));

            return Task.CompletedTask;
        }, context);

        await _next(context);
    }
}
