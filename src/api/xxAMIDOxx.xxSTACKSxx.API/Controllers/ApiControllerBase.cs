using System;
using Microsoft.AspNetCore.Mvc;

namespace xxAMIDOxx.xxSTACKSxx.API.Controllers
{
    /// <summary>
    /// Category related operations
    /// </summary>
    [Consumes("application/json")]
    [Produces("application/json")]
    [ApiExplorerSettings(GroupName = "Category")]
    public class ApiControllerBase : ControllerBase
    {
        //TODO: populate correlation from headers
        public Guid CorrellationId { get; set; }
    }
}
