using System.Net;
using System.Text.Json;

namespace TaskManagementApi.Middleware;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            // Pass the request down the pipeline
            await _next(context);
        }
        catch (Exception ex)
        {
            // Any unhandled exception lands here — return a clean JSON error
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        context.Response.ContentType = "application/json";

        // Map exception messages to correct HTTP status codes
        context.Response.StatusCode = ex.Message switch
        {
            var m when m.Contains("not found", StringComparison.OrdinalIgnoreCase)      => (int)HttpStatusCode.NotFound,
            var m when m.Contains("Invalid credentials", StringComparison.OrdinalIgnoreCase) => (int)HttpStatusCode.Unauthorized,
            var m when m.Contains("already taken", StringComparison.OrdinalIgnoreCase)  => (int)HttpStatusCode.Conflict,
            _                                                                            => (int)HttpStatusCode.InternalServerError
        };

        var response = JsonSerializer.Serialize(new
        {
            statusCode = context.Response.StatusCode,
            message    = ex.Message
        });

        return context.Response.WriteAsync(response);
    }
}
