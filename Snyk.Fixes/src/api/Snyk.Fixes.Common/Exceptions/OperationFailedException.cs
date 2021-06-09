using System;
using Amido.Stacks.Core.Exceptions;
using Amido.Stacks.Core.Operations;
using Snyk.Fixes.Common.Operations;

namespace Snyk.Fixes.Common.Exceptions
{
    [Serializable]
    public class OperationFailedException : ApplicationExceptionBase
    {
        public OperationFailedException(ExceptionCode exceptionCode, 
                   OperationCode operationCode, 
                   Guid correlationId, 
                   string message) 
                   : base((int) exceptionCode, (int) operationCode, 
                       correlationId, 
                       message)
               {
                   HttpStatusCode = (int)ExceptionCodeToHttpStatusCodeConverter.GetHttpStatusCode((int)exceptionCode);
               }
       
               public override int ExceptionCode { get; protected set; }
       
               public static void Raise(IOperationContext context, Guid localhostId, string message)
               {
                   var exception = new OperationFailedException(
                       Exceptions.ExceptionCode.OperationFailed, 
                       (OperationCode) context.OperationCode,
                       context.CorrelationId,
                       message
                       );
                   exception.Data["localhostId"] = localhostId;
                   throw exception;
               }
    }
}
