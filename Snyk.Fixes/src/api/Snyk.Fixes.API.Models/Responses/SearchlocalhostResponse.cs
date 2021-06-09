using System.Collections.Generic;

namespace Snyk.Fixes.API.Models.Responses
{
    /// <summary>
    /// Response model used by Searchlocalhost api endpoint
    /// </summary>
    public class SearchlocalhostResponse
    {
        /// <example>10</example>
        public int Size { get; set; }

        /// <example>0</example>
        public int Offset { get; set; }

        /// <summary>
        /// Contains the items returned from the search
        /// </summary>
        public List<SearchlocalhostResultItem> Results { get; set; }
    }
}
