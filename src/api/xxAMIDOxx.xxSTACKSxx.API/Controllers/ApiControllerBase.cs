using System;
using Microsoft.AspNetCore.Mvc;

namespace xxAMIDOxx.xxSTACKSxx.API.Controllers
{
    /// <summary>
    /// Category related operations
    /// </summary>
    [ApiController]
    [Consumes("application/json")]
    [Produces("application/json")]
    public class ApiControllerBase : ControllerBase
    {
        //TODO: populate correlation from headers
        public Guid CorrellationId { get; set; }
    }
}
