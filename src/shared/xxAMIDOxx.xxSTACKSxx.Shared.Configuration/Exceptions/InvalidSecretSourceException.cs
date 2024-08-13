using System;

namespace xxAMIDOxx.xxSTACKSxx.Shared.Configuration.Exceptions
{
    public class InvalidSecretSourceException : SecretException
    {
        private InvalidSecretSourceException(int exceptionCode, string message, string secretSource, string identifier, Exception innerException
            ) : base(exceptionCode, message, secretSource, identifier, innerException)
        {
        }

        public static void Raise(string source, string identifier, Exception exception = null)
        {
            throw new InvalidSecretSourceException(
                (int)ExceptionIds.InvalidSecretDefinition,
                $"The secret source is invalid. Source '{source ?? "(null)"}' identifier '{identifier ?? "(null)"}'",
                source, identifier,
                exception
            );
        }
    }
}
