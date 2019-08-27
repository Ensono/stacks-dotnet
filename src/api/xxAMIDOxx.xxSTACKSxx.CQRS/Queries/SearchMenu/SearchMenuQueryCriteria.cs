using System;
using Amido.Stacks.Application.CQRS.Queries;

namespace xxAMIDOxx.xxSTACKSxx.CQRS.Queries.SearchMenu
{
    public class SearchMenuQueryCriteria : IQueryCriteria
    {
        public int OperationCode => (int)Common.Operations.OperationCode.SearchMenu;

        public Guid CorrelationId { get; }

        public string SearchText { get; }

        public Guid? RestaurantId { get; }

        public int? PageSize { get; }

        public int? PageNumber { get; }

        public SearchMenuQueryCriteria(Guid correlationId, string searchText, Guid? restaurantId, int? pageSize, int? pageNumber)
        {
            CorrelationId = correlationId;
            SearchText = searchText;
            RestaurantId = restaurantId;
            PageSize = pageSize;
            PageNumber = pageNumber;
        }
    }
}
