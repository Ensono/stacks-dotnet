using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Amido.Stacks.Application.CQRS.Queries;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using xxAMIDOxx.xxSTACKSxx.API.Models;
using Query = xxAMIDOxx.xxSTACKSxx.CQRS.Queries.GetMenuById;

namespace xxAMIDOxx.xxSTACKSxx.API.Controllers
{
    /// <summary>
    /// Menu related operations
    /// </summary>
    [Produces("application/json")]
    [Consumes("application/json")]
    [ApiExplorerSettings(GroupName = "Menu")]
    [ApiController]
    public class GetMenuController : ControllerBase
    {
        IQueryHandler<Query.GetMenuByIdQueryCriteria, Query.Menu> queryHandler;

        public GetMenuController(IQueryHandler<Query.GetMenuByIdQueryCriteria, Query.Menu> queryHandler)
        {
            this.queryHandler = queryHandler;
        }

        /// <summary>
        /// Get a menu
        /// </summary>
        /// <remarks>By passing the menu id, you can get access to available categories and items in the menu </remarks>
        /// <param name="id">menu id</param>
        /// <response code="200">Menu</response>
        /// <response code="400">Bad Request</response>
        /// <response code="404">Resource not found</response>
        [HttpGet("/v1/menu/{id}")]
        [ProducesResponseType(typeof(Menu), 200)]
        public async Task<IActionResult> GetMenu([FromRoute][Required]Guid id)
        {
            //TODO: Uncomment the next line to return response 400 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // return StatusCode(400);

            var result = await queryHandler.ExecuteAsync(new Query.GetMenuByIdQueryCriteria() { Id = id });

            if (result == null)
                return StatusCode(404);

            return new ObjectResult(result);
        }
    }
}
