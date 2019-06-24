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
    /// Item related operations
    /// </summary>
    [Produces("application/json")]
    [ApiExplorerSettings(GroupName = "Item")]
    public class AddMenuItemController : ControllerBase
    { 
        /// <summary>
        /// Create an item to a category in the menu
        /// </summary>
        /// <remarks>Adds a new item to a category in the menu</remarks>
        /// <param name="id">menu id</param>
        /// <param name="categoryId">Id for Category being removed</param>
        /// <param name="body">Category being added</param>
        /// <response code="201">Resource created</response>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized, Access token is missing or invalid</response>
        /// <response code="403">Forbidden, the user does not have permission to execute this operation</response>
        /// <response code="409">Conflict, an item already exists</response>
        [HttpPost("/v1/menu/{id}/category/{categoryId}/items/")]
        [ValidateModelState]
        [SwaggerResponse(statusCode: 201, type: typeof(ResourceCreated), description: "Resource created")]
        public virtual IActionResult AddMenuItem([FromRoute][Required]Guid? id, [FromRoute][Required]Guid? categoryId, [FromBody]CreateOrUpdateMenuItem body)
        { 
            //TODO: Uncomment the next line to return response 201 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // return StatusCode(201, default(InlineResponse201));

            //TODO: Uncomment the next line to return response 400 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // return StatusCode(400);

            //TODO: Uncomment the next line to return response 401 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // return StatusCode(401);

            //TODO: Uncomment the next line to return response 403 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // return StatusCode(403);

            //TODO: Uncomment the next line to return response 409 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // return StatusCode(409);
            string exampleJson = null;
            
                        var example = exampleJson != null
                        ? JsonConvert.DeserializeObject<ResourceCreated>(exampleJson)
                        : default(ResourceCreated);            //TODO: Change the data returned
            return new ObjectResult(example);
        }
    }
}
