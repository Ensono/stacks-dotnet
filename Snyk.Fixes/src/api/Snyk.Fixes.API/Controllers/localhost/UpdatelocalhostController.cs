using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Amido.Stacks.Application.CQRS.Commands;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Snyk.Fixes.API.Models.Requests;
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
    public class UpdatelocalhostController : ApiControllerBase
    {
        readonly ICommandHandler<Updatelocalhost, bool> commandHandler;

        public UpdatelocalhostController(ICommandHandler<Updatelocalhost, bool> commandHandler)
        {
            this.commandHandler = commandHandler;
        }


        /// <summary>
        /// Update a localhost
        /// </summary>
        /// <remarks>Update a localhost with new information</remarks>
        /// <param name="id">localhost id</param>
        /// <param name="body">localhost being updated</param>
        /// <response code="204">No Content</response>
        /// <response code="400">Bad Request</response>
        /// <response code="404">Resource not found</response>
        [HttpPut("/v1/localhost/{id}")]
        [Authorize]
        public async Task<IActionResult> Updatelocalhost([FromRoute][Required]Guid id, [FromBody]UpdatelocalhostRequest body)
        {
            // NOTE: Please ensure the API returns the response codes annotated above

            await commandHandler.HandleAsync(
                new Updatelocalhost()
                {
                    localhostId = id,
                    Name = body.Name,
                    Description = body.Description,
                    Enabled = body.Enabled
                });

            return StatusCode(204);
        }
    }
}
