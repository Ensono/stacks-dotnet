using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Amido.Stacks.Application.CQRS.Commands;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using xxAMIDOxx.xxSTACKSxx.API.Models;
using xxAMIDOxx.xxSTACKSxx.CQRS.Commands;

namespace xxAMIDOxx.xxSTACKSxx.API.Controllers
{
    /// <summary>
    /// Category related operations
    /// </summary>
    [Consumes("application/json")]
    [Produces("application/json")]
    [ApiExplorerSettings(GroupName = "Category")]
    public class AddMenuCategoryController : ApiControllerBase
    {
        ICommandHandler<CreateCategory, Guid> commandHandler;

        public AddMenuCategoryController(ICommandHandler<CreateCategory, Guid> commandHandler)
        {
            this.commandHandler = commandHandler;
        }

        /// <summary>
        /// Create a category in the menu
        /// </summary>
        /// <remarks>Adds a category to menu</remarks>
        /// <param name="id">menu id</param>
        /// <param name="body">Category being added</param>
        /// <response code="200">Resource created</response>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized, Access token is missing or invalid</response>
        /// <response code="403">Forbidden, the user does not have permission to execute this operation</response>
        /// <response code="404">Resource not found</response>
        /// <response code="409">Conflict, an item already exists</response>
        [HttpPost("/v1/menu/{id}/category/")]
        [ProducesResponseType(typeof(ResourceCreated), 200)]
        public async Task<IActionResult> AddMenuCategory([FromRoute][Required]Guid id, [FromBody]CreateOrUpdateCategory body)
        {
            //TODO: Uncomment the next line to return response 201 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // return StatusCode(201, default(InlineResponse201));

            //TODO: Uncomment the next line to return response 400 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // return StatusCode(400);

            //TODO: Uncomment the next line to return response 401 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // return StatusCode(401);

            //TODO: Uncomment the next line to return response 403 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // return StatusCode(403);

            //TODO: Uncomment the next line to return response 409 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // return StatusCode(409);

            var categoryId = await commandHandler.HandleAsync(
                new CreateCategory()
                {
                    CorrelationId = base.CorrellationId,
                    MenuId = id,
                    Name = body.Name,
                    Description = body.Description
                });

            return new OkObjectResult(new ResourceCreated(categoryId));
        }
    }
}
