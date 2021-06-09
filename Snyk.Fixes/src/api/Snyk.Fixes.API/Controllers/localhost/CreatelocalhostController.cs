using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Amido.Stacks.Application.CQRS.Commands;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Snyk.Fixes.API.Models.Requests;
using Snyk.Fixes.API.Models.Responses;
using Snyk.Fixes.CQRS.Commands;

namespace Snyk.Fixes.API.Controllers
{
    /// <summary>
    /// localhost related operations
    /// </summary>
    [Consumes("application/json")]
    [Produces("application/json")]
    [ApiExplorerSettings(GroupName = "localhost")]
    [ApiController]
    public class CreatelocalhostController : ApiControllerBase
    {
        readonly ICommandHandler<Createlocalhost, Guid> commandHandler;

        public CreatelocalhostController(ICommandHandler<Createlocalhost, Guid> commandHandler)
        {
            this.commandHandler = commandHandler;
        }

        /// <summary>
        /// Create a localhost
        /// </summary>
        /// <remarks>Adds a localhost</remarks>
        /// <param name="body">localhost being created</param>
        /// <response code="201">Resource created</response>
        /// <response code="400">Bad Request</response>
        /// <response code="409">Conflict, an item already exists</response>
        [HttpPost("/v1/localhost/")]
        [Authorize]
        [ProducesResponseType(typeof(ResourceCreatedResponse), 201)]
        public async Task<IActionResult> Createlocalhost([Required][FromBody]CreatelocalhostRequest body)
        {
            // NOTE: Please ensure the API returns the response codes annotated above

            var id = await commandHandler.HandleAsync(
                new Createlocalhost(
                        correlationId: GetCorrelationId(),
                        tenantId: body.TenantId, //Should check if user logged-in owns it
                        name: body.Name,
                        description: body.Description,
                        enabled: body.Enabled
                    )
                );

            return new CreatedAtActionResult(
                    "Getlocalhost", "GetlocalhostById", new
                    {
                        id = id
                    }, new ResourceCreatedResponse(id)
            );
        }
    }
}
