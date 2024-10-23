using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using xxENSONOxx.xxSTACKSxx.Shared.Abstractions.Commands;
using xxENSONOxx.xxSTACKSxx.API.Models.Requests;
using xxENSONOxx.xxSTACKSxx.CQRS.Commands;

namespace xxENSONOxx.xxSTACKSxx.API.Controllers.Menu;

/// <summary>
/// Menu related operations
/// </summary>
[Produces("application/json")]
[Consumes("application/json")]
[ApiExplorerSettings(GroupName = "Menu")]
[ApiController]
public class UpdateMenuController(ICommandHandler<UpdateMenu, bool> commandHandler) : ApiControllerBase
{
    readonly ICommandHandler<UpdateMenu, bool> commandHandler = commandHandler ?? throw new ArgumentNullException(nameof(commandHandler));
    
    /// <summary>
    /// Update a menu
    /// </summary>
    /// <remarks>Update a menu with new information</remarks>
    /// <param name="id">menu id</param>
    /// <param name="body">Menu being updated</param>
    /// <response code="204">No Content</response>
    /// <response code="400">Bad Request</response>
    /// <response code="404">Resource not found</response>
    [HttpPut("/v1/menu/{id}")]
    [Authorize]
    public async Task<IActionResult> UpdateMenu([FromRoute][Required] Guid id, [FromBody] UpdateMenuRequest body)
    {
        await commandHandler.HandleAsync(
            new UpdateMenu
            {
                MenuId = id,
                Name = body.Name,
                Description = body.Description,
                Enabled = body.Enabled
            });

        return StatusCode(204);
    }
}
