using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace test.Controllers
{
    [Route("[controller]")]
    [Route("api/[controller]")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get(string path = null)
        {
            if(string.IsNullOrEmpty(path))
                path = Environment.CurrentDirectory;

            List<string> filesAndDirecotories = new List<string>();

            filesAndDirecotories.AddRange(System.IO.Directory.GetDirectories(path));
            filesAndDirecotories.AddRange(System.IO.Directory.GetFiles(path));
            
            return filesAndDirecotories;
        }
    }
}
