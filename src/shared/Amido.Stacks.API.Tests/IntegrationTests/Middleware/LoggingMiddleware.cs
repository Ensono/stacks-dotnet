using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Amido.Stacks.API.Tests.IntegrationTests.Middleware;

/// <summary>
/// Middleware intended to be called after the Correlation ID Middleware when testing.
/// This middleware writes logs that are checked to ensure that the Correlation ID
/// property has been included.
/// </summary>
public class LoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILoggerFactory _loggerFactory;


    public LoggingMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
    {
        _next = next;
        _loggerFactory = loggerFactory;

    }

    public async Task Invoke(HttpContext context)
    {
        var logger = _loggerFactory.CreateLogger(nameof(LoggingMiddleware));

        logger.LogInformation(MiddlewareStartingLogMessage);
        await _next(context);
        logger.LogInformation(MiddlewareCompleteLogMessage);
    }


    public static string MiddlewareStartingLogMessage = "Inner middleware starting.";
    public static string MiddlewareCompleteLogMessage = "Inner middleware complete.";
}