using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace xxAMIDOxx.xxSTACKSxx.API.Controllers
{
    [Route("/")]
    [ApiController]
    public class RoutesController : ControllerBase
    {
        private readonly IActionDescriptorCollectionProvider _provider;

        public RoutesController(IActionDescriptorCollectionProvider provider) {
            _provider = provider;
        }

        // GET api/values
        [HttpGet]
        public ActionResult Get()
        {
            var routes = _provider.ActionDescriptors.Items.Select(x => new { 
                Action = x.RouteValues["Action"], 
                Controller = x.RouteValues["Controller"], 
                Name = x.AttributeRouteInfo.Name, 
                Template = x.AttributeRouteInfo.Template 
            }).ToList();

            return Ok(
                routes
                    .OrderBy(x => x.Controller)
                    .ThenBy(x => x.Action)
                    .ThenBy(x => x.Template)
                    .Select(r => new {
                        Method = r.Action.ToUpper(),
                        Path = $"/{r.Template}"
                    })
                );
        }
    }
}
