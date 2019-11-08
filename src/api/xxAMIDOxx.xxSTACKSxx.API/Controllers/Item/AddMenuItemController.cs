﻿using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Amido.Stacks.Application.CQRS.Commands;
using Microsoft.AspNetCore.Mvc;
using xxAMIDOxx.xxSTACKSxx.API.Models.Requests;
using xxAMIDOxx.xxSTACKSxx.API.Models.Responses;
using xxAMIDOxx.xxSTACKSxx.CQRS.Commands;

namespace xxAMIDOxx.xxSTACKSxx.API.Controllers
{
    /// <summary>
    /// Item related operations
    /// </summary>
    [Consumes("application/json")]
    [Produces("application/json")]
    [ApiExplorerSettings(GroupName = "Item")]
    public class AddMenuItemController : ApiControllerBase
    {
        ICommandHandler<CreateMenuItem, Guid> commandHandler;

        public AddMenuItemController(ICommandHandler<CreateMenuItem, Guid> commandHandler)
        {
            this.commandHandler = commandHandler;
        }

        /// <summary>
        /// Create an item to a category in the menu
        /// </summary>
        /// <remarks>Adds a new item to a category in the menu</remarks>
        /// <param name="id">menu id</param>
        /// <param name="categoryId">Id for Category being removed</param>
        /// <param name="body">Category being added</param>
        /// <response code="201">Resource created</response>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized, Access token is missing or invalid</response>
        /// <response code="403">Forbidden, the user does not have permission to execute this operation</response>
        /// <response code="404">Resource not found</response>
        /// <response code="409">Conflict, an item already exists</response>
        [HttpPost("/v1/menu/{id}/category/{categoryId}/items/")]
        [ProducesResponseType(typeof(ResourceCreatedResponse), 201)]
        public async Task<IActionResult> AddMenuItem([FromRoute][Required]Guid id, [FromRoute][Required]Guid categoryId, [FromBody]CreateItemRequest body)
        {
            // NOTE: Please ensure the API returns the response codes annotated above

            var menuItemId = await commandHandler.HandleAsync(
                new CreateMenuItem(
                    correlationId: CorrelationId,
                    menuId: id,
                    categoryId: categoryId,
                    name: body.Name,
                    description: body.Description,
                    price: body.Price,
                    available: body.Available
                )
            );

            return new OkObjectResult(new ResourceCreatedResponse(menuItemId));
        }
    }
}
