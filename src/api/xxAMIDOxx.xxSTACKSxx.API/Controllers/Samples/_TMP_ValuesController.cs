using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace xxAMIDOxx.xxSTACKSxx.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "Samples")]
    public class ValuesController : ControllerBase
    {
        List<string> values = new List<string>() { "value1", "value2" };

        string valueFromConfig = string.Empty;
        public ValuesController(IConfiguration configuration, IOptionsMonitor<ValuesOptions> options)
        {
            values.Add(configuration.GetSection("TestConfig").Value);

            values.AddRange(options.CurrentValue.Items);
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

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
