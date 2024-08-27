using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using xxENSONOxx.xxSTACKSxx.API.Models.Requests;

namespace xxENSONOxx.xxSTACKSxx.API.Controllers;

/// <summary>
/// Item related operations
/// </summary>
[Consumes("application/json")]
[Produces("application/json")]
[ApiExplorerSettings(GroupName = "Item")]
public class UpdateMenuItemController : ApiControllerBase
{
    public UpdateMenuItemController()
    {
    }

    /// <summary>
    /// Update an item in the menu
    /// </summary>
    /// <remarks>Update an item in the menu</remarks>
    /// <param name="id">Id for menu</param>
    /// <param name="categoryId">Id for Category where the item resides</param>
    /// <param name="itemId">Id for item being updated</param>
    /// <param name="body">Item being changed</param>
    /// <response code="204">No Content</response>
    /// <response code="400">Bad Request</response>
    /// <response code="404">Resource not found</response>
    [HttpPut("/v1/menu/{id}/category/{categoryId}/items/{itemId}")]
    [Authorize]
    public async Task<IActionResult> UpdateMenuItem([FromRoute][Required] Guid id, [FromRoute][Required] Guid categoryId, [FromRoute][Required] Guid itemId, [FromBody] UpdateItemRequest body)
    {
        // NOTE: Please ensure the API returns the response codes annotated above
        await Task.CompletedTask; // Your async code will be here

        return StatusCode(204);
    }
}
