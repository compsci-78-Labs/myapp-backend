namespace WebApi;

using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

public sealed class GlobalExceptionHandler(
    ILogger<GlobalExceptionHandler> logger)
    : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext context,
        Exception exception,
        CancellationToken cancellationToken)
    {
        logger.LogError(
            exception,
            "Unhandled exception occurred. TraceId: {TraceId}",
            context.TraceIdentifier);

        var problem = new ProblemDetails
        {
            Status = StatusCodes.Status500InternalServerError,
            Title = "Internal Server Error",
            Detail = "An unexpected error occurred. Please try again later.",
            Instance = context.Request.Path
        };

        problem.Extensions["traceId"] = context.TraceIdentifier;

        context.Response.StatusCode =
            StatusCodes.Status500InternalServerError;

        await context.Response.WriteAsJsonAsync(
            problem,
            cancellationToken);

        return true;
    }
}