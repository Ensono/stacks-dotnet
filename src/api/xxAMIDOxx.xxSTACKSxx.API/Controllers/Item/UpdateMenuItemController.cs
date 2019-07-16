using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using xxAMIDOxx.xxSTACKSxx.API.Models;

namespace xxAMIDOxx.xxSTACKSxx.API.Controllers
{
    /// <summary>
    /// Item related operations
    /// </summary>
    [Consumes("application/json")]
    [Produces("application/json")]
    [ApiExplorerSettings(GroupName = "Item")]
    public class UpdateMenuItemController : ControllerBase
    {
        /// <summary>
        /// Update an item in the menu
        /// </summary>
        /// <remarks>Update an  item in the menu</remarks>
        /// <param name="id">Id for menu</param>
        /// <param name="categoryId">Id for Category</param>
        /// <param name="itemId">Id for item being updated</param>
        /// <param name="body">Category being added</param>
        /// <response code="204">No Content</response>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized, Access token is missing or invalid</response>
        /// <response code="403">Forbidden, the user does not have permission to execute this operation</response>
        /// <response code="404">Resource not found</response>
        [HttpPut("/v1/menu/{id}/category/{categoryId}/items/{itemId}")]
        public virtual IActionResult UpdateMenuItem([FromRoute][Required]Guid id, [FromRoute][Required]Guid categoryId, [FromRoute][Required]Guid itemId, [FromBody]CreateOrUpdateMenuItem body)
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
