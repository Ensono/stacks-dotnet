using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Amido.Stacks.Application.CQRS.Commands;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Snyk.Fixes.API.Models.Requests;
using Snyk.Fixes.CQRS.Commands;

namespace Snyk.Fixes.API.Controllers
{
    /// <summary>
    /// Item related operations
    /// </summary>
    [Consumes("application/json")]
    [Produces("application/json")]
    [ApiExplorerSettings(GroupName = "Item")]
    public class UpdatelocalhostItemController : ApiControllerBase
    {
        readonly ICommandHandler<UpdatelocalhostItem, bool> commandHandler;

        public UpdatelocalhostItemController(ICommandHandler<UpdatelocalhostItem, bool> commandHandler)
        {
            this.commandHandler = commandHandler;
        }

        /// <summary>
        /// Update an item in the localhost
        /// </summary>
        /// <remarks>Update an  item in the localhost</remarks>
        /// <param name="id">Id for localhost</param>
        /// <param name="categoryId">Id for Category</param>
        /// <param name="itemId">Id for item being updated</param>
        /// <param name="body">Category being added</param>
        /// <response code="204">No Content</response>
        /// <response code="400">Bad Request</response>
        /// <response code="404">Resource not found</response>
        [HttpPut("/v1/localhost/{id}/category/{categoryId}/items/{itemId}")]
        [Authorize]
        public async Task<IActionResult> UpdatelocalhostItem([FromRoute][Required]Guid id, [FromRoute][Required]Guid categoryId, [FromRoute][Required]Guid itemId, [FromBody]UpdateItemRequest body)
        {
            // NOTE: Please ensure the API returns the response codes annotated above

            await commandHandler.HandleAsync(
                new UpdatelocalhostItem(
                    correlationId: GetCorrelationId(),
                    localhostId: id,
                    categoryId: categoryId,
                    localhostItemId: itemId,
                    name: body.Name,
                    description: body.Description,
                    price: body.Price,
                    available: body.Available
                )
            );

            return StatusCode(204);
        }
    }
}
