using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Amido.Stacks.Application.CQRS.Commands;
using Microsoft.AspNetCore.Mvc;
using xxAMIDOxx.xxSTACKSxx.CQRS.Commands;

namespace xxAMIDOxx.xxSTACKSxx.API.Controllers
{
    /// <summary>
    /// Menu related operations
    /// </summary>
    [Produces("application/json")]
    [Consumes("application/json")]
    [ApiExplorerSettings(GroupName = "Menu")]
    [ApiController]
    public class DeleteMenuController : ApiControllerBase
    {
        readonly ICommandHandler<DeleteMenu, bool> commandHandler;

        public DeleteMenuController(ICommandHandler<DeleteMenu, bool> commandHandler)
        {
            this.commandHandler = commandHandler;
        }

        /// <summary>
        /// Removes a menu with all its categories and items
        /// </summary>
        /// <remarks>Remove a menu from a restaurant</remarks>
        /// <param name="id">menu id</param>
        /// <response code="204">No Content</response>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized, Access token is missing or invalid</response>
        /// <response code="403">Forbidden, the user does not have permission to execute this operation</response>
        /// <response code="404">Resource not found</response>
        [HttpDelete("/v1/menu/{id}")]
        public async Task<IActionResult> DeleteMenu([FromRoute][Required]Guid id)
        {
            // NOTE: Please ensure the API returns the response codes annotated above

            await commandHandler.HandleAsync(new DeleteMenu(id));
            return StatusCode(204);
        }
    }
}
