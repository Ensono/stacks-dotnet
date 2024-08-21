using Microsoft.AspNetCore.Builder;

namespace xxAMIDOxx.xxSTACKSxx.Shared.API.Middleware
{
    public static class CorrelationIdMiddlewareExtensions
    {
        public static void UseCorrelationId(this IApplicationBuilder app)
        {
            app.UseMiddleware<CorrelationIdMiddleware>();
        }
    }
}
