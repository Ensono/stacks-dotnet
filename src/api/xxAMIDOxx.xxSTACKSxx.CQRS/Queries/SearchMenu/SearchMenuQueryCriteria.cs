using System;
using Amido.Stacks.Application.CQRS.Queries;

namespace xxAMIDOxx.xxSTACKSxx.CQRS.Queries.SearchMenu
{
    public class SearchMenuQueryCriteria : IQueryCriteria
    {
        public int OperationCode => (int)Common.Operations.OperationCode.SearchMenu;

        public Guid CorrelationId { get; }

        public int SearchText { get; set; }

        public Guid? RestaurantId { get; set; }
    }
}
