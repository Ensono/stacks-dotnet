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
    /// Category related operations
    /// </summary>
    [Consumes("application/json")]
    [Produces("application/json")]
    [ApiExplorerSettings(GroupName = "Category")]
    public class UpdatelocalhostCategoryController : ApiControllerBase
    {
        readonly ICommandHandler<UpdateCategory, bool> commandHandler;

        public UpdatelocalhostCategoryController(ICommandHandler<UpdateCategory, bool> commandHandler)
        {
            this.commandHandler = commandHandler;
        }

        /// <summary>
        /// Update a category in the localhost
        /// </summary>
        /// <remarks>Update a category to localhost</remarks>
        /// <param name="id">localhost id</param>
        /// <param name="categoryId">Id for Category being removed</param>
        /// <param name="body">Category being added</param>
        /// <response code="204">No Content</response>
        /// <response code="400">Bad Request</response>
        /// <response code="404">Resource not found</response>
        /// <response code="409">Conflict, an item already exists</response>
        [HttpPut("/v1/localhost/{id}/category/{categoryId}")]
        [Authorize]
        public async Task<IActionResult> UpdatelocalhostCategory([FromRoute][Required]Guid id, [FromRoute][Required]Guid categoryId, [FromBody]UpdateCategoryRequest body)
        {
            // NOTE: Please ensure the API returns the response codes annotated above

            await commandHandler.HandleAsync(
                new UpdateCategory(
                    correlationId: GetCorrelationId(),
                    localhostId: id,
                    categoryId: categoryId,
                    name: body.Name,
                    description: body.Description
                )
            );

            return StatusCode(204);
        }
    }
}
