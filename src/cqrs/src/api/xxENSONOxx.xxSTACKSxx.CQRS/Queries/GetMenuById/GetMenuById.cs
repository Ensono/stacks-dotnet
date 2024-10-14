using System;
using xxENSONOxx.xxSTACKSxx.Abstractions.Queries;

namespace xxENSONOxx.xxSTACKSxx.CQRS.Queries.GetMenuById;

public class GetMenuById : IQueryCriteria
{
    public int OperationCode => (int)Common.Operations.OperationCode.GetMenuById;

    public Guid CorrelationId { get; }

    public Guid Id { get; set; }
}
