using System.Collections.Generic;

namespace xxENSONOxx.xxSTACKSxx.API.FunctionalTests.Models;

public class SearchResult
{
    public int size { get; set; }
    public int offset { get; set; }
    public List<SearchResultItem> results { get; set; }
}
