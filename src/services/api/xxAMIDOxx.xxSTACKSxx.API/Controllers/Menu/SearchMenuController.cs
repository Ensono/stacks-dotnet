using System.ComponentModel.DataAnnotations;
using Amido.Stacks.API.Validators;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using xxAMIDOxx.xxSTACKSxx.CQRS.Queries.SearchMenu;

namespace xxAMIDOxx.xxSTACKSxx.API.Controllers
{
    /// <summary>
    /// Menu related operations
    /// </summary>
    [Produces("application/json")]
    [ApiExplorerSettings(GroupName = "Menu")]
    [ApiController]
    public class SearchMenuController : ControllerBase
    {
        /// <summary>
        /// Get or search a list of menus
        /// </summary>
        /// <remarks>By passing in the appropriate options, you can search for available menus in the system </remarks>
        /// <param name="search">pass an optional search string for looking up menus</param>
        /// <param name="offset">number of records to skip for pagination</param>
        /// <param name="size">maximum number of records to return</param>
        /// <response code="200">search results matching criteria</response>
        /// <response code="400">bad request</response>
        [ValidateModelState]
        [HttpGet("/v1/menu/")]
        [ProducesResponseType(typeof(SearchMenuResult), 200)]
        public virtual IActionResult SearchMenu([FromQuery]string search, [FromQuery]int? offset = 0, [FromQuery][Range(0, 50)]int? size = 20)
        {
            //TODO: Uncomment the next line to return response 200 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // return StatusCode(200, default(SearchResult));

            //TODO: Uncomment the next line to return response 400 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // return StatusCode(400);

            string exampleJson = null;

            var example = exampleJson != null
            ? JsonConvert.DeserializeObject<SearchMenuResult>(exampleJson)
            : default(SearchMenuResult);            //TODO: Change the data returned
            return new ObjectResult(example);
        }
    }
}
