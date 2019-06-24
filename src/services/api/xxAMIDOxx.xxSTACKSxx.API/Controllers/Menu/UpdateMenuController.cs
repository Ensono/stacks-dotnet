using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;
using xxAMIDOxx.xxSTACKSxx.Models;
using xxAMIDOxx.xxSTACKSxx.API.Attributes;

namespace xxAMIDOxx.xxSTACKSxx.API.Controllers
{
    /// <summary>
    /// Menu related operations
    /// </summary>
    [Produces("application/json")]
    [ApiExplorerSettings(GroupName = "Menu")]
    public class UpdateMenuController : ControllerBase
    { 
        /// <summary>
        /// Update a menu
        /// </summary>
        /// <remarks>Update a menu with new information</remarks>
        /// <param name="id">menu id</param>
        /// <param name="body">Menu being updated</param>
        /// <response code="204">No Content</response>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized, Access token is missing or invalid</response>
        /// <response code="403">Forbidden, the user does not have permission to execute this operation</response>
        /// <response code="404">Resource not found</response>
        [HttpPut("/v1/menu/{id}")]
        [ValidateModelState]
        public virtual IActionResult UpdateMenu([FromRoute][Required]Guid id, [FromBody]CreateOrUpdateMenu body)
        { 
            //TODO: Uncomment the next line to return response 204 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // return StatusCode(204);

            //TODO: Uncomment the next line to return response 400 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // return StatusCode(400);

            //TODO: Uncomment the next line to return response 401 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // return StatusCode(401);

            //TODO: Uncomment the next line to return response 403 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // return StatusCode(403);

            //TODO: Uncomment the next line to return response 404 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // return StatusCode(404);

            throw new NotImplementedException();
        }
    }
}
