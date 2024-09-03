using System;
using System.Diagnostics;
using System.Threading.Tasks;
using xxENSONOxx.xxSTACKSxx.Shared.API.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;

namespace xxENSONOxx.xxSTACKSxx.Shared.API.Middleware
{
    public class CorrelationIdMiddleware(RequestDelegate next, IOptions<CorrelationIdConfiguration> options)
    {
        private readonly RequestDelegate _next;
        private readonly CorrelationIdConfiguration _options;
        private static readonly ActivitySource ActivitySource = new("xxENSONOxx.xxSTACKSxx");

        public async Task InvokeAsync(HttpContext context)
        {
            var correlationId = GetOrSetCorrelationId(context);

            using (var activity = ActivitySource.StartActivity("CorrelationIdMiddleware"))
            {
                activity?.SetTag(_options.HeaderName, correlationId.ToString());
                await _next(context);
            }
        }

        private StringValues GetOrSetCorrelationId(HttpContext context)
        {
            var correlationIdProvided = context.Request.Headers.TryGetValue(_options.HeaderName, out var correlationId);

            if (!correlationIdProvided)
            {
                correlationId = new StringValues(Guid.NewGuid().ToString());

                context.Request.Headers.Append(_options.HeaderName, correlationId);
            }

            if (_options.IncludeInResponse)
            {
                context.Response.OnStarting(() =>
                {
                    if (!context.Response.Headers.ContainsKey(_options.HeaderName))
                    {
                        context.Response.Headers.Append(_options.HeaderName, correlationId);
                    }

                    return Task.CompletedTask;
                });
            }

            return correlationId;
        }
    }
}
