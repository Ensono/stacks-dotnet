using System;
using xxAMIDOxx.xxSTACKSxx.Shared.Application.CQRS.Queries;

namespace xxAMIDOxx.xxSTACKSxx.CQRS.Queries.GetMenuById;

public class GetMenuById : IQueryCriteria
{
    public int OperationCode => (int)Common.Operations.OperationCode.GetMenuById;

    public Guid CorrelationId { get; }

    public Guid Id { get; set; }
}
