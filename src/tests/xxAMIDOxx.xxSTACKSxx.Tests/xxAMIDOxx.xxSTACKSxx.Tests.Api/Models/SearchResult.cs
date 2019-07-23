using System.Collections.Generic;

namespace xxAMIDOxx.xxSTACKSxx.Tests.Api.Models
{
    public class SearchResult
    {
        public int size { get; set; }
        public int offset { get; set; }
        public List<SearchResultItem> results { get; set; }
    }
}
