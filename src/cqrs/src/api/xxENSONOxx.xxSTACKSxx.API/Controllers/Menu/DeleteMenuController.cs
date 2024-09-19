using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using xxENSONOxx.xxSTACKSxx.CQRS.Commands;
using xxENSONOxx.xxSTACKSxx.Shared.Application.CQRS.Abstractions.Commands;

namespace xxENSONOxx.xxSTACKSxx.API.Controllers;

/// <summary>
/// Menu related operations
/// </summary>
[Produces("application/json")]
[Consumes("application/json")]
[ApiExplorerSettings(GroupName = "Menu")]
[ApiController]
public class DeleteMenuController(ICommandHandler<DeleteMenu, bool> commandHandler) : ApiControllerBase
{
    readonly ICommandHandler<DeleteMenu, bool> commandHandler = commandHandler ?? throw new ArgumentNullException(nameof(commandHandler));

    /// <summary>
    /// Removes a menu with all its categories and items
    /// </summary>
    /// <remarks>Remove a menu from a restaurant</remarks>
    /// <param name="id">menu id</param>
    /// <response code="204">No Content</response>
    /// <response code="400">Bad Request</response>
    /// <response code="404">Resource not found</response>
    [HttpDelete("/v1/menu/{id}")]
    [Authorize]
    public async Task<IActionResult> DeleteMenu([FromRoute][Required] Guid id)
    {
        // NOTE: Please ensure the API returns the response codes annotated above

        await commandHandler.HandleAsync(new DeleteMenu(id));
        return StatusCode(204);
    }
}
