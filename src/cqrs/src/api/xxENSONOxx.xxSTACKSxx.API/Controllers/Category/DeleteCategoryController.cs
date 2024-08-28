using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using xxENSONOxx.xxSTACKSxx.Shared.Application.CQRS.Commands;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using xxENSONOxx.xxSTACKSxx.CQRS.Commands;

namespace xxENSONOxx.xxSTACKSxx.API.Controllers;

/// <summary>
/// Category related operations
/// </summary>
[Consumes("application/json")]
[Produces("application/json")]
[ApiExplorerSettings(GroupName = "Category")]
public class DeleteCategoryController(ICommandHandler<DeleteCategory, bool> commandHandler) : ApiControllerBase
{
    readonly ICommandHandler<DeleteCategory, bool> commandHandler = commandHandler ?? throw new ArgumentNullException(nameof(commandHandler));


    /// <summary>
    /// Removes a category and its items from menu
    /// </summary>
    /// <remarks>Removes a category and its items from menu</remarks>
    /// <param name="id">menu id</param>
    /// <param name="categoryId">Id for Category being removed</param>
    /// <response code="204">No Content</response>
    /// <response code="400">Bad Request</response>
    /// <response code="404">Resource not found</response>
    [HttpDelete("/v1/menu/{id}/category/{categoryId}")]
    [Authorize]
    public async Task<IActionResult> DeleteCategory([FromRoute][Required] Guid id, [FromRoute][Required] Guid categoryId)
    {
        // NOTE: Please ensure the API returns the response codes annotated above

        await commandHandler.HandleAsync(
            new DeleteCategory(
                correlationId: GetCorrelationId(),
                menuId: id,
                categoryId: categoryId
            )
        );

        return StatusCode(204);
    }
}