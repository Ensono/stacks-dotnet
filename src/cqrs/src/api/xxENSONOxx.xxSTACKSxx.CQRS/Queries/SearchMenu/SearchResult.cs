using System.Collections.Generic;

namespace xxENSONOxx.xxSTACKSxx.CQRS.Queries.SearchMenu;

public class SearchMenuResult
{
    public int? PageSize { get; set; }

    public int? PageNumber { get; set; }

    public IEnumerable<SearchMenuResultItem> Results { get; set; }
}
