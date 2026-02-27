namespace WebApplication1.Middleware;

public class AuthenticationMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<AuthenticationMiddleware> _logger;
    private const string ApiKeyHeader = "X-Api-Key";
    private const string ValidApiKey = "my-secret-api-key-123";

    public AuthenticationMiddleware(RequestDelegate next, ILogger<AuthenticationMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Skip authentication for OpenAPI/Swagger endpoints
        if (context.Request.Path.StartsWithSegments("/openapi"))
        {
            await _next(context);
            return;
        }

        if (!context.Request.Headers.TryGetValue(ApiKeyHeader, out var extractedApiKey))
        {
            _logger.LogWarning("API key missing from request to {Path}", context.Request.Path);
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsJsonAsync(new { error = "API key is missing. Provide it via the X-Api-Key header." });
            return;
        }

        if (!ValidApiKey.Equals(extractedApiKey))
        {
            _logger.LogWarning("Invalid API key used for request to {Path}", context.Request.Path);
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsJsonAsync(new { error = "Invalid API key." });
            return;
        }

        await _next(context);
    }
}
