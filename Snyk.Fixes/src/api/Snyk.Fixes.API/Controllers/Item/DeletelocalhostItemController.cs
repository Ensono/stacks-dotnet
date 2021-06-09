using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Amido.Stacks.Application.CQRS.Commands;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Snyk.Fixes.CQRS.Commands;

namespace Snyk.Fixes.API.Controllers
{
    /// <summary>
    /// Item related operations
    /// </summary>
    [Consumes("application/json")]
    [Produces("application/json")]
    [ApiExplorerSettings(GroupName = "Item")]
    public class DeletelocalhostItemController : ApiControllerBase
    {
        readonly ICommandHandler<DeletelocalhostItem, bool> commandHandler;

        public DeletelocalhostItemController(ICommandHandler<DeletelocalhostItem, bool> commandHandler)
        {
            this.commandHandler = commandHandler;
        }

        /// <summary>
        /// Removes an item from localhost
        /// </summary>
        /// <remarks>Removes an item from localhost</remarks>
        /// <param name="id">localhost id</param>
        /// <param name="categoryId">Category ID</param>
        /// <param name="itemId">Id for Item being removed</param>
        /// <response code="204">No Content</response>
        /// <response code="400">Bad Request</response>
        /// <response code="404">Resource not found</response>
        [HttpDelete("/v1/localhost/{id}/category/{categoryId}/items/{itemId}")]
        [Authorize]
        public async Task<IActionResult> DeletelocalhostItem([FromRoute][Required]Guid id, [FromRoute][Required]Guid categoryId, [FromRoute][Required]Guid itemId)
        {
            // NOTE: Please ensure the API returns the response codes annotated above

            await commandHandler.HandleAsync(
                new DeletelocalhostItem(
                    correlationId: GetCorrelationId(),
                    localhostId: id,
                    categoryId: categoryId,
                    localhostItemId: itemId
                )
            );

            return StatusCode(204);
        }
    }
}
