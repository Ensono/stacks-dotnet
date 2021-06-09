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
    using Microsoft.AspNetCore.Http;

    /// <summary>
    /// Item related operations
    /// </summary>
    [Consumes("application/json")]
    [Produces("application/json")]
    [ApiExplorerSettings(GroupName = "Item")]
    public class AddlocalhostItemController : ApiControllerBase
    {
        readonly ICommandHandler<CreatelocalhostItem, Guid> commandHandler;

        public AddlocalhostItemController(ICommandHandler<CreatelocalhostItem, Guid> commandHandler)
        {
            this.commandHandler = commandHandler;
        }

        /// <summary>
        /// Create an item to a category in the localhost
        /// </summary>
        /// <remarks>Adds a new item to a category in the localhost</remarks>
        /// <param name="id">localhost id</param>
        /// <param name="categoryId">Id for Category being removed</param>
        /// <param name="body">Category being added</param>
        /// <response code="201">Resource created</response>
        /// <response code="400">Bad Request</response>
        /// <response code="404">Resource not found</response>
        /// <response code="409">Conflict, an item already exists</response>
        [HttpPost("/v1/localhost/{id}/category/{categoryId}/items/")]
        [Authorize]
        [ProducesResponseType(typeof(ResourceCreatedResponse), 201)]
        public async Task<IActionResult> AddlocalhostItem([FromRoute][Required]Guid id, [FromRoute][Required]Guid categoryId, [FromBody]CreateItemRequest body)
        {
            // NOTE: Please ensure the API returns the response codes annotated above

            var localhostItemId = await commandHandler.HandleAsync(
                new CreatelocalhostItem(
                    correlationId: GetCorrelationId(),
                    localhostId: id,
                    categoryId: categoryId,
                    name: body.Name,
                    description: body.Description,
                    price: body.Price,
                    available: body.Available
                )
            );

            return StatusCode(StatusCodes.Status201Created, new ResourceCreatedResponse(localhostItemId));
        }
    }
}
