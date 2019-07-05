using System.ComponentModel.DataAnnotations;
using Amido.Stacks.API.Validators;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using xxAMIDOxx.xxSTACKSxx.API.Models;
using xxAMIDOxx.xxSTACKSxx.CQRS.Commands;

namespace xxAMIDOxx.xxSTACKSxx.API.Controllers
{
    /// <summary>
    /// Menu related operations
    /// </summary>
    [Consumes("application/json")]
    [Produces("application/json")]
    [ApiExplorerSettings(GroupName = "Menu")]
    [ApiController]
    public class CreateMenuController : ControllerBase
    {
        /// <summary>
        /// Create a menu
        /// </summary>
        /// <remarks>Adds a menu</remarks>
        /// <param name="body">Menu being added</param>
        /// <response code="201">Resource created</response>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized, Access token is missing or invalid</response>
        /// <response code="403">Forbidden, the user does not have permission to execute this operation</response>
        /// <response code="409">Conflict, an item already exists</response>
        [ValidateModelState]
        [HttpPost("/v1/menu/")]
        [ProducesResponseType(typeof(ResourceCreated), 201)]
        public virtual IActionResult CreateMenu([Required][FromBody]CreateMenu body)
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
