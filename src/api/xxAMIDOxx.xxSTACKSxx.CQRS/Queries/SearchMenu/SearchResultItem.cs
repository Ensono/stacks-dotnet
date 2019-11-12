using System;

namespace xxAMIDOxx.xxSTACKSxx.CQRS.Queries.SearchMenu
{
    public class SearchMenuResultItem
    {
        public Guid? Id { get; set; }

        public Guid RestaurantId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public bool? Enabled { get; set; }
    }
}
