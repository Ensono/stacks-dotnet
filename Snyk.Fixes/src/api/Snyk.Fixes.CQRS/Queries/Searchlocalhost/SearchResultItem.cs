using System;

namespace Snyk.Fixes.CQRS.Queries.Searchlocalhost
{
    public class SearchlocalhostResultItem
    {
        public Guid? Id { get; set; }

        public Guid RestaurantId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public bool? Enabled { get; set; }
    }
}
