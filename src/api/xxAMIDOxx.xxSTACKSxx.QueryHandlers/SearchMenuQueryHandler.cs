using System;
using System.Threading.Tasks;
using Amido.Stacks.Application.CQRS.Queries;
using xxAMIDOxx.xxSTACKSxx.CQRS.Queries.SearchMenu;

namespace xxAMIDOxx.xxSTACKSxx.QueryHandlers
{
    public class SearchMenuQueryHandler : IQueryHandler<SearchMenuQueryCriteria, SearchMenuResult>
    {
        public Task<SearchMenuResult> ExecuteAsync(SearchMenuQueryCriteria criteria)
        {
            //TODO: get search menu from that matches the criteria and map to result

            return Task.FromResult(
                new SearchMenuResult()
                {
                    Offset = 0,
                    Size = 20,
                    Results = new System.Collections.Generic.List<SearchMenuResultItem>()
                    {
                        new SearchMenuResultItem()
                        {
                            Id = Guid.NewGuid(),
                            RestaurantId = Guid.NewGuid(),
                            Name = "Lunch Menu (dummy)",
                            Description = "This is a dummy item",
                            Enabled = true
                        }
                    }
                }
            );
        }
    }
}
