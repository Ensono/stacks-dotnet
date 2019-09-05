﻿using System.Collections.Generic;

namespace xxAMIDOxx.xxSTACKSxx.API.Models
{
    /// <summary>
    /// Response model used by SearchMenu api endpoint
    /// </summary>
    public partial class SearchMenuResult
    {
        /// <example>10</example>
        public int Size { get; set; }

        /// <example>0</example>
        public int Offset { get; set; }

        /// <summary>
        /// Contains the items returned from the search
        /// </summary>
        public List<SearchMenuResultItem> Results { get; set; }
    }
}
