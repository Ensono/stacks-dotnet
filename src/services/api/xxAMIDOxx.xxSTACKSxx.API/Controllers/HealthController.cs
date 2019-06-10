using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace xxAMIDOxx.xxSTACKSxx.API.Controllers
{
    [Route("/")] //temporary, should redirect to swagger in the future
    [Route("/[controller]")]
    [ApiController]
    public class HealthController: ControllerBase
    {
        [HttpGet]
        public ActionResult Get()
        {
            return Ok("ok");
        }
    }
}
