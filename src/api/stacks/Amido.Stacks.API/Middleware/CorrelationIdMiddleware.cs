﻿using System;
using System.Threading.Tasks;
using Amido.Stacks.API.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Serilog.Context;

namespace Amido.Stacks.API.Middleware
{
    public class CorrelationIdMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly CorrelationIdConfiguration _options;

        public CorrelationIdMiddleware(RequestDelegate next, IOptions<CorrelationIdConfiguration> options)
        {
            _next = next;
            _options = options.Value;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var correlationId = GetOrSetCorrelationId(context);

            //There is a bug in .Net Core 2.1+ that replaces CorrelationId by null, until fixed we will add attributes "twice"
            using (LogContext.PushProperty("CorrelationId", correlationId.ToString()))
            using (LogContext.PushProperty(_options.HeaderName, correlationId.ToString()))
            {
                await _next(context);
            }
        }

        private StringValues GetOrSetCorrelationId(HttpContext context)
        {
            var correlationIdProvided = context.Request.Headers.TryGetValue(_options.HeaderName, out var correlationId);

            if (!correlationIdProvided)
            {
                correlationId = new StringValues(Guid.NewGuid().ToString());

                context.Request.Headers.Add(_options.HeaderName, correlationId);
            }

            if (_options.IncludeInResponse)
            {
                context.Response.OnStarting(() =>
                {
                    if (!context.Response.Headers.ContainsKey(_options.HeaderName))
                    {
                        context.Response.Headers.Add(_options.HeaderName, correlationId);
                    }

                    return Task.CompletedTask;
                });
            }

            return correlationId;
        }
    }
}
