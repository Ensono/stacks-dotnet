using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
}