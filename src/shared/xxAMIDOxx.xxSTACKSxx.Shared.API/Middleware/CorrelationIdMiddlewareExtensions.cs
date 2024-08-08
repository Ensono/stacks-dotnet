using Microsoft.AspNetCore.Builder;

namespace Amido.Stacks.API.Middleware
{
    public static class CorrelationIdMiddlewareExtensions
    {
        public static void UseCorrelationId(this IApplicationBuilder app)
        {
            app.UseMiddleware<CorrelationIdMiddleware>();
        }
    }
}
