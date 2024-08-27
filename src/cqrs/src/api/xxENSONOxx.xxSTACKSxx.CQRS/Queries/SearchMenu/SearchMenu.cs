using System;
using xxENSONOxx.xxSTACKSxx.Shared.Application.CQRS.Queries;

namespace xxENSONOxx.xxSTACKSxx.CQRS.Queries.SearchMenu;

public class SearchMenu(Guid correlationId, string searchText, Guid? restaurantId, int? pageSize, int? pageNumber)
    : IQueryCriteria
{
    public int OperationCode => (int)Common.Operations.OperationCode.SearchMenu;

    public Guid CorrelationId { get; } = correlationId;

    public string SearchText { get; } = searchText;

    public Guid? TenantId { get; } = restaurantId;

    public int? PageSize { get; } = pageSize;

    public int? PageNumber { get; } = pageNumber;
}
