using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Snyk.Fixes.API.Models.Responses;

namespace Snyk.Fixes.API.Controllers
{
    /// <summary>
    /// localhost related operations
    /// </summary>
    [Produces("application/json")]
    [Consumes("application/json")]
    [ApiExplorerSettings(GroupName = "localhost")]
    [ApiController]
    public class GetlocalhostByIdV2Controller : ApiControllerBase
    {
        /// <summary>
        /// Get a localhost
        /// </summary>
        /// <remarks>By passing the localhost id, you can get access to available categories and items in the localhost </remarks>
        /// <param name="id">localhost id</param>
        /// <response code="200">localhost</response>
        /// <response code="400">Bad Request</response>
        /// <response code="404">Resource not found</response>
        [HttpGet("/v2/localhost/{id}")]
        [Authorize]
        [ProducesResponseType(typeof(localhost), 200)]
        public virtual IActionResult GetlocalhostV2([FromRoute][Required]Guid id)
        {
            // NOTE: Please ensure the API returns the response codes annotated above

            return BadRequest();
        }
    }
}
