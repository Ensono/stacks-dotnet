using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using xxENSONOxx.xxSTACKSxx.Shared.Core.Exceptions;
using BadResult = xxENSONOxx.xxSTACKSxx.API.Models.Responses.BadResult;

namespace xxENSONOxx.xxSTACKSxx.API.Middleware;

public static class ExceptionHandlerMiddlewareExtensions
{
    public static void UseCustomExceptionHandler(this IApplicationBuilder app, ILogger logger)
    {
        app.UseExceptionHandler(appError =>
        {
            appError.Run(async context =>
            {
                context.Response.ContentType = "application/json";
                string message = "Internal Server Error.";

                var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                if (contextFeature != null)
                {
                    switch (contextFeature.Error)
                    {
                        case ApplicationExceptionBase aex:
                            message = aex.Message;
                            context.Response.StatusCode = aex.HttpStatusCode;
                            break;
                        case InfrastructureExceptionBase iex:
                            context.Response.StatusCode = iex.HttpStatusCode;
                            break;
                        default:
                            break;
                    }

                    logger.LogError(contextFeature.Error, "Request failed");
                }

                logger.LogWarning("Exception: {Message}", contextFeature?.Error?.Message ?? message);

                await context.Response.WriteAsync(new BadResult(context.Response.StatusCode)
                {
                    Title = message,
                    TraceId = context.TraceIdentifier
                }.ToString());
            });
        });
    }
}