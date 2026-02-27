using System.Diagnostics;

namespace WebApplication1.Middleware;

public class LoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<LoggingMiddleware> _logger;

    public LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var stopwatch = Stopwatch.StartNew();
        var method = context.Request.Method;
        var path = context.Request.Path;

        _logger.LogInformation("Incoming request: {Method} {Path}", method, path);

        await _next(context);

        stopwatch.Stop();
        var statusCode = context.Response.StatusCode;

        _logger.LogInformation(
            "Response: {Method} {Path} responded {StatusCode} in {ElapsedMs}ms",
            method, path, statusCode, stopwatch.ElapsedMilliseconds);
    }
}
