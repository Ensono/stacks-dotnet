using System;

namespace xxENSONOxx.xxSTACKSxx.Shared.Messaging.Azure.ServiceBus.Exceptions
{
    public class InvalidSecretDefinitionException : SecretException
    {
        private InvalidSecretDefinitionException(int exceptionCode, string message, string secretSource, string identifier, Exception innerException) 
            : base(exceptionCode, message, secretSource, identifier, innerException)
        {
        }

        public static void Raise(string source, string identifier, Exception exception = null)
        {
            throw new InvalidSecretDefinitionException(
                (int)ExceptionIds.InvalidSecretDefinition,
                $"The secret definition loaded from the configuration is invalid. Source '{source ?? "(null)"}' identifier '{identifier ?? "(null)"}'",
                source, identifier,
                exception
            );
        }
    }
}
