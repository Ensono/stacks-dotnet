using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using xxAMIDOxx.xxSTACKSxx.API.Models.Responses;

namespace xxAMIDOxx.xxSTACKSxx.API.Controllers;

/// <summary>
/// Menu related operations
/// </summary>
[Produces("application/json")]
[Consumes("application/json")]
[ApiExplorerSettings(GroupName = "Menu")]
[ApiController]
public class GetMenuByIdV2Controller : ApiControllerBase
{
    /// <summary>
    /// Get a menu
    /// </summary>
    /// <remarks>By passing the menu id, you can get access to available categories and items in the menu </remarks>
    /// <param name="id">menu id</param>
    /// <response code="200">Menu</response>
    /// <response code="400">Bad Request</response>
    /// <response code="404">Resource not found</response>
    [HttpGet("/v2/menu/{id}")]
    [Authorize]
    [ProducesResponseType(typeof(Menu), 200)]
    public virtual IActionResult GetMenuV2([FromRoute][Required] Guid id)
    {
        // NOTE: Please ensure the API returns the response codes annotated above

        return BadRequest();
    }
}
