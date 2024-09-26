using System;

namespace xxENSONOxx.xxSTACKSxx.API.Exceptions;

public abstract class InfrastructureExceptionBase : Exception, IException
{
    public InfrastructureExceptionBase(string message, Exception ex) : base(message, ex)
    {
        Data["ExceptionCode"] = ExceptionCode;
    }

    public abstract int ExceptionCode { get; protected set; } //this should not have a set

    public virtual int HttpStatusCode { get; protected set; } = 500; //this should not have a set
}
