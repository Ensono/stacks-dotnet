using System;
using xxENSONOxx.xxSTACKSxx.Shared.Abstractions.Operations;

namespace xxENSONOxx.xxSTACKSxx.Common.Exceptions;

[Serializable]
public class OperationFailedException : ApplicationExceptionBase
{
    public OperationFailedException(ExceptionCode exceptionCode,
        OperationCode operationCode,
        Guid correlationId,
        string message)
        : base((int)operationCode,
            correlationId,
            message)
    {
        ExceptionCode = (int)exceptionCode;

        HttpStatusCode = (int)ExceptionCodeToHttpStatusCodeConverter.GetHttpStatusCode((int)exceptionCode);
    }

    public override int ExceptionCode { get; protected set; }

    public static void Raise(IOperationContext context, Guid menuId, string message)
    {
        var exception = new OperationFailedException(
            Exceptions.ExceptionCode.OperationFailed,
            (OperationCode)context.OperationCode,
            context.CorrelationId,
            message
        );
        exception.Data["MenuId"] = menuId;
        throw exception;
    }
}
