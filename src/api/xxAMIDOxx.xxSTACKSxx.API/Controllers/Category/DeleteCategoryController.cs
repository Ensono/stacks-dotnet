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
    /// Category related operations
    /// </summary>
    [Consumes("application/json")]
    [Produces("application/json")]
    [ApiExplorerSettings(GroupName = "Category")]
    public class DeleteCategoryController : ApiControllerBase
    {
        ICommandHandler<DeleteCategory, bool> commandHandler;

        public DeleteCategoryController(ICommandHandler<DeleteCategory, bool> commandHandler)
        {
            this.commandHandler = commandHandler;
        }


        /// <summary>
        /// Removes a category and its items from menu
        /// </summary>
        /// <remarks>Removes a category and its items from menu</remarks>
        /// <param name="id">menu id</param>
        /// <param name="categoryId">Id for Category being removed</param>
        /// <response code="204">No Content</response>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized, Access token is missing or invalid</response>
        /// <response code="403">Forbidden, the user does not have permission to execute this operation</response>
        /// <response code="404">Resource not found</response>
        [HttpDelete("/v1/menu/{id}/category/{categoryId}")]
        public async Task<IActionResult> DeleteCategory([FromRoute][Required]Guid id, [FromRoute][Required]Guid categoryId)
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
                new DeleteCategory(
                    correlationId: CorrelationId,
                    menuId: id,
                    categoryId: categoryId
                )
            );

            return StatusCode(204);
        }
    }
}
