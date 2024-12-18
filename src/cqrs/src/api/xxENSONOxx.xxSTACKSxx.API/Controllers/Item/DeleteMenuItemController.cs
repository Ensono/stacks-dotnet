using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using xxENSONOxx.xxSTACKSxx.CQRS.Commands;
using xxENSONOxx.xxSTACKSxx.Shared.Abstractions.Commands;

namespace xxENSONOxx.xxSTACKSxx.API.Controllers.Item;

/// <summary>
/// Item related operations
/// </summary>
[Consumes("application/json")]
[Produces("application/json")]
[ApiExplorerSettings(GroupName = "Item")]
public class DeleteMenuItemController(ICommandHandler<DeleteMenuItem, bool> commandHandler) : ApiControllerBase
{
    readonly ICommandHandler<DeleteMenuItem, bool> commandHandler = commandHandler ?? throw new ArgumentNullException(nameof(commandHandler));

    /// <summary>
    /// Removes an item from menu
    /// </summary>
    /// <remarks>Removes an item from menu</remarks>
    /// <param name="id">menu id</param>
    /// <param name="categoryId">Category ID</param>
    /// <param name="itemId">Id for Item being removed</param>
    /// <response code="204">No Content</response>
    /// <response code="400">Bad Request</response>
    /// <response code="404">Resource not found</response>
    [HttpDelete("/v1/menu/{id}/category/{categoryId}/items/{itemId}")]
    [Authorize]
    public async Task<IActionResult> DeleteMenuItem([FromRoute][Required] Guid id, [FromRoute][Required] Guid categoryId, [FromRoute][Required] Guid itemId)
    {
        // NOTE: Please ensure the API returns the response codes annotated above

        await commandHandler.HandleAsync(
            new DeleteMenuItem(
                correlationId: GetCorrelationId(),
                menuId: id,
                categoryId: categoryId,
                menuItemId: itemId
            )
        );

        return StatusCode(204);
    }
}
