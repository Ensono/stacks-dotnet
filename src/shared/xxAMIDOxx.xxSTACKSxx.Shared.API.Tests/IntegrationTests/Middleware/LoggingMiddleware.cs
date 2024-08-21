using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace xxAMIDOxx.xxSTACKSxx.Shared.API.Tests.IntegrationTests.Middleware;

/// <summary>
/// Middleware intended to be called after the Correlation ID Middleware when testing.
/// This middleware writes logs that are checked to ensure that the Correlation ID
/// property has been included.
/// </summary>
public class LoggingMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
{
    public async Task Invoke(HttpContext context)
    {
        var logger = loggerFactory.CreateLogger(nameof(LoggingMiddleware));

        logger.LogInformation(MiddlewareStartingLogMessage);
        await next(context);
        logger.LogInformation(MiddlewareCompleteLogMessage);
    }


    public static string MiddlewareStartingLogMessage = "Inner middleware starting.";
    public static string MiddlewareCompleteLogMessage = "Inner middleware complete.";
}
