using System;
using Amido.Stacks.Core.Exceptions;

namespace Amido.Stacks.Configuration.Exceptions
{
    public abstract class SecretException : InfrastructureExceptionBase
    {
        public override int ExceptionCode { get; protected set; }
        public string SecretSource { get; set; }
        public string Identifier { get; set; }

        protected SecretException(int exceptionCode, string message, string secretSource, string identifier, Exception innerException) : base(message, innerException)
        {
            SecretSource = secretSource;
            Identifier = identifier;
            ExceptionCode = exceptionCode;

            Data["SecretSource"] = SecretSource;
            Data["Identifier"] = Identifier;
        }
    }
}
