using Microsoft.AspNetCore.Mvc;

namespace xxAMIDOxx.xxSTACKSxx.API.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    public class HealthController : ControllerBase
    {
        [HttpGet]
        public ActionResult Get()
        {
            // TODO: Implement proper checks for critical dependencies
            //      Health check shouls ensure the dependencies are up and running
            //      When a dependency fail to respond, the health check should return a failure response
            //      - Check database connection
            //      - Check bus connection
            //      - Check cache
            //      - Check dependent services
            // TODO: Consider Circuit-break checks as well

            //Maybe refactor this as a middleware

            /* Supporting links
             * https://docs.microsoft.com/en-us/dotnet/architecture/microservices/implement-resilient-applications/index
             * https://docs.microsoft.com/en-us/dotnet/architecture/microservices/implement-resilient-applications/monitor-app-health
             * https://microservices.io/patterns/observability/health-check-api.html
             * https://www.sohamkamani.com/blog/architecture/2018-09-06-application-health-monitoring/
             * https://docs.microsoft.com/en-us/dotnet/architecture/microservices/implement-resilient-applications/implement-circuit-breaker-pattern
             */


            return Ok("ok");
        }
    }
}
