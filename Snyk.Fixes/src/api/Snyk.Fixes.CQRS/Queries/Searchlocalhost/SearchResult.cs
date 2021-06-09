using System.Collections.Generic;

namespace Snyk.Fixes.CQRS.Queries.Searchlocalhost
{
    public class SearchlocalhostResult
    {
        public int? PageSize { get; set; }

        public int? PageNumber { get; set; }

        public IEnumerable<SearchlocalhostResultItem> Results { get; set; }
    }
}
