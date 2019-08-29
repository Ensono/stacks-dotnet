﻿using System;
using System.Linq;
using System.Threading.Tasks;
using Amido.Stacks.Application.CQRS.Queries;
using Amido.Stacks.Data.Documents.Abstractions;
using xxAMIDOxx.xxSTACKSxx.CQRS.Queries.SearchMenu;
using xxAMIDOxx.xxSTACKSxx.Domain;

namespace xxAMIDOxx.xxSTACKSxx.Application.QueryHandlers
{
    public class SearchMenuQueryHandler : IQueryHandler<SearchMenuQueryCriteria, SearchMenuResult>
    {
        IDocumentSearch<Menu> storage;

        public SearchMenuQueryHandler(IDocumentSearch<Menu> storage)
        {
            this.storage = storage;
        }

        public async Task<SearchMenuResult> ExecuteAsync(SearchMenuQueryCriteria criteria)
        {
            if (criteria == null)
                throw new ArgumentException("A valid SearchMenuQueryCriteria os required!");

            int pageSize = 10;
            int pageNumber = 1;
            var searchTerm = string.Empty;
            Guid restaurantId = criteria.RestaurantId.HasValue ? criteria.RestaurantId.Value : Guid.Empty;

            if (criteria.PageSize.HasValue && criteria.PageSize > 0)
                pageSize = criteria.PageSize.Value;

            if (criteria.PageNumber.HasValue && criteria.PageNumber > 0)
                pageNumber = criteria.PageNumber.Value;

            if (!string.IsNullOrEmpty(criteria.SearchText))
                searchTerm = criteria.SearchText.Trim();

            bool restaurantIdProvided = criteria.RestaurantId.HasValue;

            var results = await storage.Search(
                itemFilter =>
                    (string.IsNullOrEmpty(searchTerm) || itemFilter.Name.Contains(searchTerm)) &&
                    //Nullable types must have a value when passed to a seach, this is why we convert it to non nullable and pass a boolean check
                    (!restaurantIdProvided || itemFilter.RestaurantId == restaurantId)
                    ,
                null,
                pageSize,
                pageNumber);

            var result = new SearchMenuResult();
            result.PageSize = pageSize;
            result.PageNumber = pageNumber;

            if (!results.IsSuccessful)
                return result;

            result.Results = results.Content.Select(i => new SearchMenuResultItem()
            {
                Id = i.Id,
                RestaurantId = i.RestaurantId,
                Name = i.Name,
                Description = i.Description,
                Enabled = i.Enabled
            });


            return result;
        }
    }
}
