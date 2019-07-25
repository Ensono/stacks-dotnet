using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Amido.Stacks.Application.CQRS.Commands;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using xxAMIDOxx.xxSTACKSxx.CQRS.Commands;

namespace xxAMIDOxx.xxSTACKSxx.API.Controllers
{
    /// <summary>
    /// Item related operations
    /// </summary>
    [Consumes("application/json")]
    [Produces("application/json")]
    [ApiExplorerSettings(GroupName = "Item")]
    public class DeleteMenuItemController : ApiControllerBase
    {
        ICommandHandler<DeleteMenuItem, bool> commandHandler;

        public DeleteMenuItemController(ICommandHandler<DeleteMenuItem, bool> commandHandler)
        {
            this.commandHandler = commandHandler;
        }

        /// <summary>
        /// Removes an item from menu
        /// </summary>
        /// <remarks>Removes an item from menu</remarks>
        /// <param name="id">menu id</param>
        /// <param name="categoryId">Category ID</param>
        /// <param name="itemId">Id for Item being removed</param>
        /// <response code="204">No Content</response>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized, Access token is missing or invalid</response>
        /// <response code="403">Forbidden, the user does not have permission to execute this operation</response>
        /// <response code="404">Resource not found</response>
        [HttpDelete("/v1/menu/{id}/category/{categoryId}/items/{itemId}")]
        public async Task<IActionResult> DeleteMenuItem([FromRoute][Required]Guid id, [FromRoute][Required]Guid categoryId, [FromRoute][Required]Guid itemId)
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

            await commandHandler.HandleAsync(
                new DeleteMenuItem(
                    base.CorrellationId,
                    id,
                    categoryId,
                    itemId
                )
            );

            return StatusCode(204);
        }
    }
}
