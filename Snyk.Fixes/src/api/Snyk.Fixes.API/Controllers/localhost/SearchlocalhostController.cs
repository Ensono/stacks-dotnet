using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Amido.Stacks.Application.CQRS.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Snyk.Fixes.CQRS.Queries.Searchlocalhost;

namespace Snyk.Fixes.API.Controllers
{
    /// <summary>
    /// localhost related operations
    /// </summary>
    [Produces("application/json")]
    [Consumes("application/json")]
    [ApiExplorerSettings(GroupName = "localhost")]
    [ApiController]
    public class SearchlocalhostController : ApiControllerBase
    {
        readonly IQueryHandler<Searchlocalhost, SearchlocalhostResult> queryHandler;

        public SearchlocalhostController(IQueryHandler<Searchlocalhost, SearchlocalhostResult> queryHandler)
        {
            this.queryHandler = queryHandler;
        }


        /// <summary>
        /// Get or search a list of localhosts
        /// </summary>
        /// <remarks>By passing in the appropriate options, you can search for available localhosts in the system </remarks>
        /// <param name="searchTerm">pass an optional search string for looking up localhosts</param>
        /// <param name="RestaurantId">id of restaurant to look up for localhost's</param>
        /// <param name="pageNumber">page number</param>
        /// <param name="pageSize">maximum number of records to return per page</param>
        /// <response code="200">search results matching criteria</response>
        /// <response code="400">bad request</response>
        [HttpGet("/v1/localhost/")]
        [Authorize]
        [ProducesResponseType(typeof(SearchlocalhostResult), 200)]
        public async Task<IActionResult> Searchlocalhost(
            [FromQuery]string searchTerm, 
            [FromQuery]Guid? RestaurantId, 
            [FromQuery][Range(0, 50)]int? pageSize = 20, 
            [FromQuery]int? pageNumber = 1)
        {
            // NOTE: Please ensure the API returns the response codes annotated above

            var criteria = new Searchlocalhost(
                correlationId: GetCorrelationId(),
                searchText: searchTerm,
                restaurantId: RestaurantId,
                pageSize: pageSize,
                pageNumber: pageNumber
            );

            var results = await queryHandler.ExecuteAsync(criteria);

            return new ObjectResult(results); //TOOD: we need a mapping here
        }
    }
}
