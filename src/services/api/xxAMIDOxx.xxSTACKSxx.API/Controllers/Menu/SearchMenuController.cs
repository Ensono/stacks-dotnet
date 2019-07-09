using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;
using xxAMIDOxx.xxSTACKSxx.Models;
using xxAMIDOxx.xxSTACKSxx.API.Attributes;

namespace xxAMIDOxx.xxSTACKSxx.API.Controllers
{
    /// <summary>
    /// Menu related operations
    /// </summary>
    [Produces("application/json")]
    [ApiExplorerSettings(GroupName = "Menu")]
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
        /// <response code="404">Resource not found</response>
        [HttpGet("/v1/menu/")]
        [ValidateModelState]
        [SwaggerResponse(statusCode: 200, type: typeof(SearchResult), description: "search results matching criteria")]
        public virtual IActionResult SearchMenu([FromQuery]string search, [FromQuery]int? offset, [FromQuery][Range(0, 50)]int? size)
        { 
            //TODO: Uncomment the next line to return response 200 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // return StatusCode(200, default(SearchResult));

            //TODO: Uncomment the next line to return response 400 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // return StatusCode(400);

            //TODO: Uncomment the next line to return response 404 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // return StatusCode(404);
            string exampleJson = null;
            
                        var example = exampleJson != null
                        ? JsonConvert.DeserializeObject<SearchResult>(exampleJson)
                        : default(SearchResult);            //TODO: Change the data returned
            return new ObjectResult(example);
        }
    }
}
