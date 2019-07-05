using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace xxAMIDOxx.xxSTACKSxx.CQRS.Queries.SearchMenu
{
    public partial class SearchMenuResult
    {
        public int? Size { get; set; }

        public int? Offset { get; set; }
        public List<SearchMenuResultItem> Results { get; set; }
    }
}
