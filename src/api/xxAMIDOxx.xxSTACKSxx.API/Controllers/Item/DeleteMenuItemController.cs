using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace xxAMIDOxx.xxSTACKSxx.API.Controllers;

/// <summary>
/// Item related operations
/// </summary>
[Consumes("application/json")]
[Produces("application/json")]
[ApiExplorerSettings(GroupName = "Item")]
public class DeleteMenuItemController : ApiControllerBase
{
    public DeleteMenuItemController()
    {
    }

    /// <summary>
    /// Removes an item from menu
    /// </summary>
    /// <remarks>Removes an item from menu</remarks>
    /// <param name="id">Menu Id</param>
    /// <param name="categoryId">Category Id</param>
    /// <param name="itemId">Id for Item being removed</param>
    /// <response code="204">No Content</response>
    /// <response code="400">Bad Request</response>
    /// <response code="404">Resource not found</response>
    [HttpDelete("/v1/menu/{id}/category/{categoryId}/items/{itemId}")]
    [Authorize]
    public async Task<IActionResult> DeleteMenuItem([FromRoute][Required] Guid id, [FromRoute][Required] Guid categoryId, [FromRoute][Required] Guid itemId)
    {
        // NOTE: Please ensure the API returns the response codes annotated above

        await Task.CompletedTask; // Your async code will be here

        return StatusCode(204);
    }
}
