using System;

namespace Amido.Stacks.Domain
{
    //TODO: MOVE to domain package
    public abstract class DomainException : Exception
    {
        public DomainException(string message) : base(message)
        {
        }

        public abstract int ExceptionCode { get; }
    }
}
