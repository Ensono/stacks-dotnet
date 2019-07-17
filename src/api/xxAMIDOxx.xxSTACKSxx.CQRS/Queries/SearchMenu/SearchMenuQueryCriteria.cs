using System;
using System.Collections.Generic;
using System.Text;

namespace xxAMIDOxx.xxSTACKSxx.CQRS.Queries.SearchMenu
{
    public class SearchMenuQueryCriteria
    {
        public int SearchText { get; set; }

        public Guid? RestaurantId { get; set; }
    }
}
