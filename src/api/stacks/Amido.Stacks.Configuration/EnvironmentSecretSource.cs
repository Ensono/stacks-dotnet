using System;
using Amido.Stacks.Configuration.Exceptions;

namespace Amido.Stacks.Configuration
{
    public class EnvironmentSecretSource : ISecretSource<string>
    {
        public string Source { get; }

        public EnvironmentSecretSource() : this("ENVIRONMENT") { }

        public EnvironmentSecretSource(string source)
        {
            Source = source;
        }

        public string Resolve(Secret secret)
        {
            if (secret == null)
                SecretNotDefinedException.Raise();

            if (string.IsNullOrWhiteSpace(secret.Source))
                InvalidSecretDefinitionException.Raise(secret.Source, secret.Identifier);

            if (string.IsNullOrWhiteSpace(secret.Identifier))
                InvalidSecretDefinitionException.Raise(secret.Source, secret.Identifier);

            if (secret.Source.ToUpperInvariant() != Source)
                SecretNotFoundException.Raise(secret.Source, secret.Identifier);

            var result = Environment.GetEnvironmentVariable(secret.Identifier);

            if (result != null)
                return result.Trim();

            if (!secret.Optional)
                SecretNotFoundException.Raise(secret.Source, secret.Identifier);

            return default;
        }
    }
}
