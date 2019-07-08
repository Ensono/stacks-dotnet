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
    /// Menu related operations
    /// </summary>
    [Produces("application/json")]
    [Consumes("application/json")]
    [ApiExplorerSettings(GroupName = "Menu")]
    [ApiController]
    public class UpdateMenuController : ControllerBase
    {
        ICommandHandler<UpdateMenu> commandHandler;

        public UpdateMenuController(ICommandHandler<UpdateMenu> commandHandler)
        {
            this.commandHandler = commandHandler;
        }


        /// <summary>
        /// Update a menu
        /// </summary>
        /// <remarks>Update a menu with new information</remarks>
        /// <param name="id">menu id</param>
        /// <param name="body">Menu being updated</param>
        /// <response code="204">No Content</response>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized, Access token is missing or invalid</response>
        /// <response code="403">Forbidden, the user does not have permission to execute this operation</response>
        /// <response code="404">Resource not found</response>
        [HttpPut("/v1/menu/{id}")]
        public async Task<IActionResult> UpdateMenu([FromRoute][Required]Guid id, [FromBody]CreateOrUpdateMenu body)
        {
            // THESE SHOULD BE HANDLED BY SECURITY FILTERS AND VALIDATORS

            // TODO: Uncomment the next line to return response 401 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // return StatusCode(401);

            // TODO: Uncomment the next line to return response 403 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // return StatusCode(403);

            // Handled by Validator

            // TODO: Uncomment the next line to return response 400 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // return StatusCode(400);

            // THESE CODES SHOULD BE HANDLED BY EXCEPTIONS IN THE HANDLER

            //TODO: Uncomment the next line to return response 204 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // return StatusCode(204);

            //TODO: Uncomment the next line to return response 404 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // return StatusCode(404);

            await commandHandler.HandleAsync(
                new UpdateMenu()
                {
                    Id = id,
                    Name = body.Name,
                    Description = body.Description,
                    Enabled = body.Enabled
                });

            return StatusCode(204);
        }
    }
}
