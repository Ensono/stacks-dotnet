using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using xxENSONOxx.xxSTACKSxx.API.Configuration;

namespace xxENSONOxx.xxSTACKSxx.API.Middleware
{
    public class CorrelationIdMiddleware(RequestDelegate next, IOptions<CorrelationIdConfiguration> options)
    {
        private readonly CorrelationIdConfiguration _options = options.Value;
        private static readonly ActivitySource activitySource = new("xxENSONOxx.xxSTACKSxx");

        public async Task InvokeAsync(HttpContext context)
        {
            var correlationId = GetOrSetCorrelationId(context);

            using var activity = activitySource.StartActivity("CorrelationIdMiddleware");
            activity?.SetTag(_options.HeaderName, correlationId.ToString());
            await next(context);
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
