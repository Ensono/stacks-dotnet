using System;

namespace xxAMIDOxx.xxSTACKSxx.Shared.Configuration.Exceptions
{
    public class SecretNotDefinedException : SecretException
    {
        private SecretNotDefinedException(int exceptionCode, string message, string secretSource, string identifier, Exception innerException
            ) : base(exceptionCode, message, secretSource, identifier, innerException)
        {
        }

        public static void Raise(Exception exception = null)
        {
            throw new SecretNotDefinedException(
                (int)ExceptionIds.InvalidSecretDefinition,
                $"The secret definition was null or undefined",
                null, null,
                exception
            );
        }
    }
}
