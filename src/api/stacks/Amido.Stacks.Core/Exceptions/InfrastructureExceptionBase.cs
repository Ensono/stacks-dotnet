using System;

namespace Amido.Stacks.Core.Exceptions
{
    public class InfrastructureExceptionBase : Exception, IException
    {
        public InfrastructureExceptionBase(int exceptionCode, string message) : this(exceptionCode, message, null) { }

        public InfrastructureExceptionBase(int exceptionCode, string message, Exception innerException) : base(message, innerException)
        {
            ExceptionCode = exceptionCode;

            Data["ExceptionCode"] = ExceptionCode;
        }

        public int ExceptionCode { get; }

        public virtual int HttpStatusCode => 500;
    }
}
