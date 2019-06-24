using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace xxAMIDOxx.xxSTACKSxx.Models
{
    public partial class SearchResult
    { 
        public int? Size { get; set; }

        public int? Offset { get; set; }
        public List<SearchResultItem> Results { get; set; }
    }
}
