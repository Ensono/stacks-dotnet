using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Amido.Stacks.Application.CQRS.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using xxAMIDOxx.xxSTACKSxx.API.Models.Responses;
using xxAMIDOxx.xxSTACKSxx.CQRS.Queries.SearchMenu;

namespace xxAMIDOxx.xxSTACKSxx.API.Controllers;

/// <summary>
/// Menu related operations
/// </summary>
[Produces("application/json")]
[Consumes("application/json")]
[ApiExplorerSettings(GroupName = "Menu")]
[ApiController]
public class SearchMenuController : ApiControllerBase
{
    readonly IQueryHandler<SearchMenu, SearchMenuResult> queryHandler;

    public SearchMenuController(IQueryHandler<SearchMenu, SearchMenuResult> queryHandler)
    {
        this.queryHandler = queryHandler ?? throw new ArgumentNullException(nameof(queryHandler));
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

        var criteria = new SearchMenu(
            correlationId: GetCorrelationId(),
            searchText: searchTerm,
            restaurantId: RestaurantId,
            pageSize: pageSize,
            pageNumber: pageNumber
        );

        var result = await queryHandler.ExecuteAsync(criteria);

        var response = SearchMenuResponse.FromMenuResultItem(result);

        return new ObjectResult(response);
    }
}
