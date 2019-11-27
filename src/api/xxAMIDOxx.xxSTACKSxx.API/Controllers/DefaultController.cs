using System;
using Microsoft.AspNetCore.Mvc;

namespace xxAMIDOxx.xxSTACKSxx.API.Controllers
{
    /// <summary>
    /// Endpoint to ensure we forward root requests to swagger
    /// </summary>
    [Route("/")]
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class DefaultController : ControllerBase
    {
        public IActionResult Index()
        {
            return Redirect((Environment.GetEnvironmentVariable("API_BASEPATH") ?? String.Empty) + "/swagger");
        }
    }

    //This can now be replaced by the following snipped on .Net Core 3
    /*          
        endpoints.MapGet("/",  async context => 
        {
            context.Response.Redirect((Environment.GetEnvironmentVariable("API_BASEPATH") ?? String.Empty) + "/swagger");
        });
     */
}
