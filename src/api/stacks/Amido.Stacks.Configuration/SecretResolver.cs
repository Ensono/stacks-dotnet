using System;
using System.Collections.Generic;

namespace Amido.Stacks.Configuration
{
    public class SecretResolver : ISecretResolver<string>
    {
        Dictionary<string, ISecretSource<string>> sources = new Dictionary<string, ISecretSource<string>>();

        public SecretResolver()
        {
            AddSource(new EnvironmentSecretSource());
            AddSource(new FileSecretSource());
        }

        public void AddSource(ISecretSource<string> source)
        {
            sources.Add(source.Source.ToUpperInvariant(), source);
        }

        public string ResolveSecret(Secret secret)
        {
            if (secret == null)
                throw new ArgumentNullException($"The parameter {nameof(secret)} cann't be null");

            if (string.IsNullOrWhiteSpace(secret.Source))
                throw new InvalidOperationException($"A valid secret source must be provided.");

            if (string.IsNullOrWhiteSpace(secret.Identifier))
                throw new ArgumentException($"The value '{secret.Identifier ?? "(null)"}' provided as identifiers is not valid");

            if (!sources.ContainsKey(secret.Source.ToUpperInvariant()))
            {
                throw new Exception($"Secret source '{secret.Source}' not found");
            }

            var source = sources[secret.Source.ToUpperInvariant()];


            var result = source.Resolve(secret);

            if (result != null)
                return result;

            if (secret.Optional)
                return default;
            else
                throw new Exception($"No value found for Secret '{secret.Identifier}' on source '{secret.Source}'.");
        }
    }
}
