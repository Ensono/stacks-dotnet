using Amido.Stacks.Application.CQRS;
using System;
using System.Threading.Tasks;
using xxAMIDOxx.xxSTACKSxx.Models;
using xxAMIDOxx.xxSTACKSxx.Models.Queries;

namespace xxAMIDOxx.xxSTACKSxx.QueryHandlers
{
    public class SearchMenuQueryHandler : IQueryHandler<SearchMenuQuery, SearchResult>
    {
        public Task<SearchResult> Execute(SearchMenuQuery criteria)
        {
            //TODO: get search menu from that matches the criteria and map to result

            return Task.FromResult(
                new SearchResult()
                {
                    Offset = 0,
                    Size = 20,
                    Results = new System.Collections.Generic.List<SearchResultItem>()
                    {
                        new SearchResultItem()
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
