using System;
using xxENSONOxx.xxSTACKSxx.Shared.Abstractions.Queries;

namespace xxENSONOxx.xxSTACKSxx.CQRS.Queries.GetMenuById;

public class GetMenuById : IQueryCriteria
{
    public int OperationCode => (int)Shared.Abstractions.Operations.OperationCode.GetMenuById;

    public Guid CorrelationId { get; }

    public Guid Id { get; set; }
}
