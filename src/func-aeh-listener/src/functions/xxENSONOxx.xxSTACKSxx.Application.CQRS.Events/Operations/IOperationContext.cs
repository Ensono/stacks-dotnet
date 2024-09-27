using System;

namespace xxENSONOxx.xxSTACKSxx.Application.CQRS.Events.Operations;

public interface IOperationContext
{
    /// <summary>
    /// Code of operation that triggered the current context
    /// </summary>
    int OperationCode { get; }

    /// <summary>
    /// Unique id related to the request that initiated the operation context
    /// </summary>
    Guid CorrelationId { get; }
}
