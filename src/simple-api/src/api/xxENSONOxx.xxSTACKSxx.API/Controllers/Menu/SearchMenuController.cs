using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using xxENSONOxx.xxSTACKSxx.API.Models.Responses;

namespace xxENSONOxx.xxSTACKSxx.API.Controllers;

/// <summary>
/// Menu related operations
/// </summary>
[Produces("application/json")]
[Consumes("application/json")]
[ApiExplorerSettings(GroupName = "Menu")]
[ApiController]
public class SearchMenuController : ApiControllerBase
{
    public SearchMenuController()
    {
    }


    /// <summary>
    /// Get or search a list of menus
    /// </summary>
    /// <remarks>By passing in the appropriate options, you can search for available menus in the system </remarks>
    /// <param name="searchTerm">pass an optional search string for looking up menus</param>
    /// <param name="RestaurantId">id of restaurant to look up for menu's</param>
    /// <param name="pageNumber">page number</param>
    /// <param name="pageSize">maximum number of records to return per page</param>
    /// <response code="200">search results matching criteria</response>
    /// <response code="400">bad request</response>
    [HttpGet("/v1/menu/")]
    [Authorize]
    [ProducesResponseType(typeof(SearchMenuResponse), 200)]
    public async Task<IActionResult> SearchMenu(
        [FromQuery] string searchTerm,
        [FromQuery] Guid? RestaurantId,
        [FromQuery][Range(0, 50)] int? pageSize = 20,
        [FromQuery] int? pageNumber = 1)
    {
        // NOTE: Please ensure the API returns the response codes annotated above

        var results = new SearchMenuResponse()
        {
            Offset = (pageNumber ?? 0) * (pageSize ?? 0),
            Size = (pageSize ?? 0),
            Results = new List<SearchMenuResponseItem>()
            {
                new SearchMenuResponseItem()
                {
                    Name = "Menu",
                    Description = "Menu Description",
                    Enabled = true,
                    Id = Guid.NewGuid()
                },
                new SearchMenuResponseItem()
                {
                    Name = "Menu 2",
                    Description = "Menu Description 2",
                    Enabled = true,
                    Id = Guid.NewGuid()
                }
            }
        };

        await Task.CompletedTask; // Your async code will be here

        return new ObjectResult(results);
    }
}
