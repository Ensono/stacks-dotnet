using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Amido.Stacks.Application.CQRS.Commands;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Snyk.Fixes.CQRS.Commands;

namespace Snyk.Fixes.API.Controllers
{
    /// <summary>
    /// localhost related operations
    /// </summary>
    [Produces("application/json")]
    [Consumes("application/json")]
    [ApiExplorerSettings(GroupName = "localhost")]
    [ApiController]
    public class DeletelocalhostController : ApiControllerBase
    {
        readonly ICommandHandler<Deletelocalhost, bool> commandHandler;

        public DeletelocalhostController(ICommandHandler<Deletelocalhost, bool> commandHandler)
        {
            this.commandHandler = commandHandler;
        }

        /// <summary>
        /// Removes a localhost with all its categories and items
        /// </summary>
        /// <remarks>Remove a localhost from a restaurant</remarks>
        /// <param name="id">localhost id</param>
        /// <response code="204">No Content</response>
        /// <response code="400">Bad Request</response>
        /// <response code="404">Resource not found</response>
        [HttpDelete("/v1/localhost/{id}")]
        [Authorize]
        public async Task<IActionResult> Deletelocalhost([FromRoute][Required]Guid id)
        {
            // NOTE: Please ensure the API returns the response codes annotated above

            await commandHandler.HandleAsync(new Deletelocalhost(id));
            return StatusCode(204);
        }
    }
}
