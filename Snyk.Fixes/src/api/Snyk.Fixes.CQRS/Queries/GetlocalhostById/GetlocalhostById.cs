using System;
using Amido.Stacks.Application.CQRS.Queries;

namespace Snyk.Fixes.CQRS.Queries.GetlocalhostById
{
    public class GetlocalhostById : IQueryCriteria
    {
        public int OperationCode => (int)Common.Operations.OperationCode.GetlocalhostById;

        public Guid CorrelationId { get; }

        public Guid Id { get; set; }
    }
}
