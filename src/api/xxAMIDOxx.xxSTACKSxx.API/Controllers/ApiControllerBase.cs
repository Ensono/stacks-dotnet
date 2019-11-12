using System;
using Microsoft.AspNetCore.Mvc;

namespace xxAMIDOxx.xxSTACKSxx.API.Controllers
{
    [ApiController]
    [Consumes("application/json")]
    [Produces("application/json")]
    public class ApiControllerBase : ControllerBase
    {
        [NonAction]
        public Guid GetCorrelationId()
        {
            var correlationIdProvided = this.HttpContext.Request.Headers.TryGetValue("x-correlation-id", out var correlationId);

            if (!correlationIdProvided)
                throw new ArgumentException("The correlation id couldn't be loaded");

            return Guid.Parse(correlationId.ToString());
        }
    }
}
