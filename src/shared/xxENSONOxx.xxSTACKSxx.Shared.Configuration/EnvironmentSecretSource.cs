using System;
using System.Threading.Tasks;
using xxENSONOxx.xxSTACKSxx.Shared.Configuration.Exceptions;

namespace xxENSONOxx.xxSTACKSxx.Shared.Configuration
{
    public class EnvironmentSecretSource(string source) : ISecretSource<string>
    {
        public string Source { get; } = source;

        public EnvironmentSecretSource() : this("ENVIRONMENT") { }

        public async Task<string> ResolveAsync(Secret secret)
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
                return await Task.FromResult(result.Trim());

            if (!secret.Optional)
                SecretNotFoundException.Raise(secret.Source, secret.Identifier);

            return default;
        }
    }
}
