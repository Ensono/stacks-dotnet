using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Amido.Stacks.Application.CQRS.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Snyk.Fixes.API.Models.Responses;
using Query = Snyk.Fixes.CQRS.Queries.GetlocalhostById;

namespace Snyk.Fixes.API.Controllers
{
    /// <summary>
    /// localhost related operations
    /// </summary>
    [Produces("application/json")]
    [Consumes("application/json")]
    [ApiExplorerSettings(GroupName = "localhost")]
    [ApiController]
    public class GetlocalhostByIdController : ApiControllerBase
    {
        readonly IQueryHandler<Query.GetlocalhostById, Query.localhost> queryHandler;

        public GetlocalhostByIdController(IQueryHandler<Query.GetlocalhostById, Query.localhost> queryHandler)
        {
            this.queryHandler = queryHandler;
        }

        /// <summary>
        /// Get a localhost
        /// </summary>
        /// <remarks>By passing the localhost id, you can get access to available categories and items in the localhost </remarks>
        /// <param name="id">localhost id</param>
        /// <response code="200">localhost</response>
        /// <response code="400">Bad Request</response>
        /// <response code="404">Resource not found</response>
        [HttpGet("/v1/localhost/{id}")]
        [Authorize]
        [ProducesResponseType(typeof(localhost), 200)]
        public async Task<IActionResult> Getlocalhost([FromRoute][Required]Guid id)
        {
            // NOTE: Please ensure the API returns the response codes annotated above

            var result = await queryHandler.ExecuteAsync(new Query.GetlocalhostById() { Id = id });

            if (result == null)
                return NotFound();

            return new ObjectResult(result);
        }
    }
}
