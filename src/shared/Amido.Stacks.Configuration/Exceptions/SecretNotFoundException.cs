using System;

namespace Amido.Stacks.Configuration.Exceptions
{
    public class SecretNotFoundException : SecretException
    {
        private SecretNotFoundException(int exceptionCode, string message, string secretSource, string identifier, Exception innerException
            ) : base(exceptionCode, message, secretSource, identifier, innerException)
        {
        }

        public static void Raise(string source, string identifier, Exception exception = null)
        {
            throw new SecretNotFoundException(
                (int)ExceptionIds.InvalidSecretDefinition,
                $"The secret '{identifier ?? "(null)"}' doesn't exist in the source '{source ?? "(null)"}'.",
                source, identifier,
                exception
            );
        }
    }
}
