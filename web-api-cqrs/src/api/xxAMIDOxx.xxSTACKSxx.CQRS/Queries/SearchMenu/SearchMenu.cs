using System;
using Amido.Stacks.Application.CQRS.Queries;

namespace xxAMIDOxx.xxSTACKSxx.CQRS.Queries.SearchMenu
{
    public class SearchMenu : IQueryCriteria
    {
        public int OperationCode => (int)Common.Operations.OperationCode.SearchMenu;

        public Guid CorrelationId { get; }

        public string SearchText { get; }

        public Guid? TenantId { get; }

        public int? PageSize { get; }

        public int? PageNumber { get; }

        public SearchMenu(Guid correlationId, string searchText, Guid? restaurantId, int? pageSize, int? pageNumber)
        {
            CorrelationId = correlationId;
            SearchText = searchText;
            TenantId = restaurantId;
            PageSize = pageSize;
            PageNumber = pageNumber;
        }
    }
}
