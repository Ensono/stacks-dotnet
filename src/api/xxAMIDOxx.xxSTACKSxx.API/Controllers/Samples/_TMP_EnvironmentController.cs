using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace xxAMIDOxx.xxSTACKSxx.API.Controllers
{
    [Route("env")]
    [Route("[controller]")]
    [ApiExplorerSettings(GroupName = "Samples")]
    [ApiController]
    public class EnvironmentController : ControllerBase
    {
        private bool isDevEnv = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development";

        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            if (!isDevEnv)
                NotFound();

            List<string> variables = new List<string>() { };

            var enumerator = Environment.GetEnvironmentVariables().GetEnumerator();
            while (enumerator.MoveNext())
            {
                variables.Add($"{enumerator.Key,30}:{enumerator.Value}");
            }

            return variables;
        }
    }
}
