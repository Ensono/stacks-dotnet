using System.Collections.Generic;

namespace xxENSONOxx.xxSTACKSxx.API.FunctionalTests.Models;

public class SearchResponse
{
    public int size { get; set; }
    public int offset { get; set; }
    public List<SearchResponseItem> results { get; set; }
}
