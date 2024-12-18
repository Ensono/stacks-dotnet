using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using xxENSONOxx.xxSTACKSxx.API.Models.Requests;
using xxENSONOxx.xxSTACKSxx.CQRS.Commands;
using xxENSONOxx.xxSTACKSxx.Shared.Abstractions.Commands;

namespace xxENSONOxx.xxSTACKSxx.API.Controllers.Item;

/// <summary>
/// Item related operations
/// </summary>
[Consumes("application/json")]
[Produces("application/json")]
[ApiExplorerSettings(GroupName = "Item")]
public class UpdateMenuItemController(ICommandHandler<UpdateMenuItem, bool> commandHandler) : ApiControllerBase
{
    readonly ICommandHandler<UpdateMenuItem, bool> commandHandler = commandHandler ?? throw new ArgumentNullException(nameof(commandHandler));

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
    /// <response code="404">Resource not found</response>
    [HttpPut("/v1/menu/{id}/category/{categoryId}/items/{itemId}")]
    [Authorize]
    public async Task<IActionResult> UpdateMenuItem([FromRoute][Required] Guid id, [FromRoute][Required] Guid categoryId, [FromRoute][Required] Guid itemId, [FromBody] UpdateItemRequest body)
    {
        // NOTE: Please ensure the API returns the response codes annotated above

        await commandHandler.HandleAsync(
            new UpdateMenuItem(
                correlationId: GetCorrelationId(),
                menuId: id,
                categoryId: categoryId,
                menuItemId: itemId,
                name: body.Name,
                description: body.Description,
                price: body.Price,
                available: body.Available
            )
        );

        return StatusCode(204);
    }
}
