using System;
using Amido.Stacks.Application.CQRS.Queries;

namespace Snyk.Fixes.CQRS.Queries.Searchlocalhost
{
    public class Searchlocalhost : IQueryCriteria
    {
        public int OperationCode => (int)Common.Operations.OperationCode.Searchlocalhost;

        public Guid CorrelationId { get; }

        public string SearchText { get; }

        public Guid? TenantId { get; }

        public int? PageSize { get; }

        public int? PageNumber { get; }

        public Searchlocalhost(Guid correlationId, string searchText, Guid? restaurantId, int? pageSize, int? pageNumber)
        {
            CorrelationId = correlationId;
            SearchText = searchText;
            TenantId = restaurantId;
            PageSize = pageSize;
            PageNumber = pageNumber;
        }
    }
}
