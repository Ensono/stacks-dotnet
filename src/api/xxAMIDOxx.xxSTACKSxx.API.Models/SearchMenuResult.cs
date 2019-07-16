using System.Collections.Generic;

namespace xxAMIDOxx.xxSTACKSxx.API.Models
{
    public partial class SearchMenuResult
    {
        /// <example>10</example>
        public int Size { get; set; }

        /// <example>0</example>
        public int Offset { get; set; }

        public List<SearchMenuResultItem> Results { get; set; }
    }
}
