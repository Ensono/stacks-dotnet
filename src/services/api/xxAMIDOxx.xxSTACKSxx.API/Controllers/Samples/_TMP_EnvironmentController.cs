using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace xxAMIDOxx.xxSTACKSxx.API.Controllers
{
    [Route("env")]
    [Route("[controller]")]
    [ApiExplorerSettings(GroupName = "Samples")]
    [ApiController]
    public class EnvironmentController : ControllerBase
    {
        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
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
