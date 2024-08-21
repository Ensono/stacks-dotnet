using System;
using System.Threading.Tasks;
using xxAMIDOxx.xxSTACKSxx.Shared.API.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Serilog.Context;

namespace xxAMIDOxx.xxSTACKSxx.Shared.API.Middleware
{
    public class CorrelationIdMiddleware(RequestDelegate next, IOptions<CorrelationIdConfiguration> options)
    {
        private readonly CorrelationIdConfiguration _options = options.Value;

        public async Task InvokeAsync(HttpContext context)
        {
            var correlationId = GetOrSetCorrelationId(context);

            using (LogContext.PushProperty(_options.HeaderName, correlationId.ToString()))
            {
                await next(context);
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
