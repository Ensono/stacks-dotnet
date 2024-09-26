using System;
using System.Runtime.Serialization;
using xxENSONOxx.xxSTACKSxx.API.Operations;

namespace xxENSONOxx.xxSTACKSxx.API.Exceptions;

public abstract class ApplicationExceptionBase : ApplicationException, IException, IOperationContext
{
    protected ApplicationExceptionBase(SerializationInfo info, StreamingContext context) : base(info, context) { }
    
    /// <summary>
    /// Unique identifier for this exception type used for aggregation and translation of exception messages
    /// </summary>
    public abstract int ExceptionCode { get; protected set; }

    public int OperationCode { get; }

    public Guid CorrelationId { get; }

    public virtual int HttpStatusCode { get; protected set; } = 500;
}
