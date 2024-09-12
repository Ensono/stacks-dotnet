#if (DynamoDb || CosmosDb)
using System;
using System.Linq;
using System.Threading.Tasks;
using Amazon.DynamoDBv2.DocumentModel;
using xxENSONOxx.xxSTACKSxx.Shared.Application.CQRS.Queries;
#if (DynamoDb)
using Amazon.DynamoDBv2.DocumentModel;
using xxENSONOxx.xxSTACKSxx.Infrastructure.Abstractions;
#endif
using xxENSONOxx.xxSTACKSxx.CQRS.Queries.SearchMenu;
using xxENSONOxx.xxSTACKSxx.Domain;
using xxENSONOxx.xxSTACKSxx.Infrastructure.Abstractions;

namespace xxENSONOxx.xxSTACKSxx.Application.QueryHandlers;

#if (DynamoDb)
public class SearchMenuQueryHandler : IQueryHandler<SearchMenu, SearchMenuResult>
{
    private readonly IDynamoDbObjectSearch<Menu> search;

    public SearchMenuQueryHandler(IDynamoDbObjectSearch<Menu> search)
    {
        this.search = search;
    }

    public Task<SearchMenuResult> ExecuteAsync(SearchMenu criteria)
    {
        if (criteria == null)
            throw new ArgumentException("A valid SearchMenuQueryCriteria os required!");

        // https://rules.sonarsource.com/csharp/RSPEC-4457
        return ExecuteInternalAsync(criteria);
    }

    private async Task<SearchMenuResult> ExecuteInternalAsync(SearchMenu criteria)
    {
        var searchTerm = string.Empty;

        if (!string.IsNullOrEmpty(criteria.SearchText))
            searchTerm = criteria.SearchText.Trim();

        var scanFilter = new ScanFilter();
        scanFilter.AddCondition("Name", ScanOperator.Contains, searchTerm);

        var results = await search.ScanAsync(scanFilter);

        var result = new SearchMenuResult();

        if (!results.IsSuccessful)
            return result;

        result.Results = results.Content.Select(SearchMenuResultItem.FromDomain);

        return result;
    }
}
#elif CosmosDb
public class SearchMenuQueryHandler(IDocumentSearch<Menu> storage) : IQueryHandler<SearchMenu, SearchMenuResult>
{
    public Task<SearchMenuResult> ExecuteAsync(SearchMenu criteria)
    {
        if (criteria == null)
            throw new ArgumentException("A valid SearchMenuQueryCriteria os required!");

        // https://rules.sonarsource.com/csharp/RSPEC-4457
        return ExecuteInternalAsync(criteria);
    }

    private async Task<SearchMenuResult> ExecuteInternalAsync(SearchMenu criteria)
    {
        int pageSize = 10;
        int pageNumber = 1;
        var searchTerm = string.Empty;
        Guid tenantId = criteria.TenantId.HasValue ? criteria.TenantId.Value : Guid.Empty;

        if (criteria.PageSize.HasValue && criteria.PageSize > 0)
            pageSize = criteria.PageSize.Value;

        if (criteria.PageNumber.HasValue && criteria.PageNumber > 0)
            pageNumber = criteria.PageNumber.Value;

        if (!string.IsNullOrEmpty(criteria.SearchText))
            searchTerm = criteria.SearchText.Trim();

        bool restaurantIdProvided = criteria.TenantId.HasValue;

        var results = await storage.Search(
            itemFilter =>
                (string.IsNullOrEmpty(searchTerm) || itemFilter.Name.Contains(searchTerm)) &&
                //Nullable types must have a value when passed to a search, this is why we convert it to non nullable and pass a boolean check
                (!restaurantIdProvided || itemFilter.TenantId == tenantId),
            null,
            pageSize,
            pageNumber);

        var result = new SearchMenuResult();
        result.PageSize = pageSize;
        result.PageNumber = pageNumber;

        if (!results.IsSuccessful)
            return result;

        result.Results = results.Content.Select(SearchMenuResultItem.FromDomain);

        return result;
    }
}
#endif
#endif
