using Microsoft.AspNetCore.Builder;

namespace xxENSONOxx.xxSTACKSxx.API.Middleware;

public static class CorrelationIdMiddlewareExtensions
{
    public static void UseCorrelationId(this IApplicationBuilder app)
    {
        app.UseMiddleware<CorrelationIdMiddleware>();
    }
}