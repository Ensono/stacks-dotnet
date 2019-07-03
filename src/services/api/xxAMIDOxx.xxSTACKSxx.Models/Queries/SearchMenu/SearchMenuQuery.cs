using System;
using System.Collections.Generic;
using System.Text;

namespace xxAMIDOxx.xxSTACKSxx.Models.Queries
{
    public class SearchMenuQuery
    {
        public int SearchText { get; set; }

        public Guid? RestaurantId { get; set; }
    }
}
