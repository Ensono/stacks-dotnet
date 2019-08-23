using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace xxAMIDOxx.xxSTACKSxx.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "Samples")]
    public class RoutesController : ControllerBase
    {
        private readonly IActionDescriptorCollectionProvider _provider;

        public RoutesController(IActionDescriptorCollectionProvider provider)
        {
            _provider = provider;
        }

        private bool isDevEnv = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development";

        [HttpGet]
        public ActionResult Get()
        {
            if (!isDevEnv)
                NotFound();

            var routes = _provider.ActionDescriptors.Items.Select(x => new
            {
                Action = x.RouteValues["Action"],
                Controller = x.RouteValues["Controller"],
                Name = x.AttributeRouteInfo.Name,
                Template = x.AttributeRouteInfo.Template
            }).ToList();

            return Ok(
                routes
                    .OrderBy(x => x.Template)
                    .Select(r => $"/{r.Template}")
                );
        }
    }
}
