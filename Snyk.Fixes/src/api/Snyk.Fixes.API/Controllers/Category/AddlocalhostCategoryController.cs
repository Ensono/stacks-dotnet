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
    /// Category related operations
    /// </summary>
    [Consumes("application/json")]
    [Produces("application/json")]
    [ApiExplorerSettings(GroupName = "Category")]
    public class AddlocalhostCategoryController : ApiControllerBase
    {
        readonly ICommandHandler<CreateCategory, Guid> commandHandler;

        public AddlocalhostCategoryController(ICommandHandler<CreateCategory, Guid> commandHandler)
        {
            this.commandHandler = commandHandler;
        }

        /// <summary>
        /// Create a category in the localhost
        /// </summary>
        /// <remarks>Adds a category to localhost</remarks>
        /// <param name="id">localhost id</param>
        /// <param name="body">Category being added</param>
        /// <response code="201">Resource created</response>
        /// <response code="400">Bad Request</response>
        /// <response code="404">Resource not found</response>
        /// <response code="409">Conflict, an item already exists</response>
        [HttpPost("/v1/localhost/{id}/category/")]
        [Authorize]
        [ProducesResponseType(typeof(ResourceCreatedResponse), StatusCodes.Status201Created)]
        public async Task<IActionResult> AddlocalhostCategory([FromRoute][Required]Guid id, [FromBody]CreateCategoryRequest body)
        {
            // NOTE: Please ensure the API returns the response codes annotated above

            var categoryId = await commandHandler.HandleAsync(
                new CreateCategory(
                    correlationId: GetCorrelationId(),
                    localhostId: id,
                    name: body.Name,
                    description: body.Description
                )
            );

            return StatusCode(StatusCodes.Status201Created, new ResourceCreatedResponse(categoryId));
        }
    }
}
