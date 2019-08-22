using System.Collections.Generic;

namespace xxAMIDOxx.xxSTACKSxx.CQRS.Queries.SearchMenu
{
    public partial class SearchMenuResult
    {
        public int? PageSize { get; set; }

        public int? PageNumber { get; set; }

        public IEnumerable<SearchMenuResultItem> Results { get; set; }
    }
}
