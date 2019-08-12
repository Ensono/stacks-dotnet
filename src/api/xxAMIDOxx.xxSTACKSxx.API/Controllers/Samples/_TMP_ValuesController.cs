using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace xxAMIDOxx.xxSTACKSxx.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "Samples")]
    public class ValuesController : ControllerBase
    {
        List<string> values = new List<string>() { "value1", "value2" };

        public ValuesController()
        {
        }

        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return values;
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

    }
}
