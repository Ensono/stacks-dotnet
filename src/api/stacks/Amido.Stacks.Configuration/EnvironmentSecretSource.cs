using System;
using System.Threading.Tasks;

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

        public async Task<string> ResolveAsync(Secret secret)
        {
            if (secret == null)
                throw new ArgumentNullException($"The parameter {nameof(secret)} cann't be null");

            if (secret.Source.ToUpperInvariant() != Source)
                throw new InvalidOperationException($"The source {secret.Source} does not match the source {Source}");

            if (string.IsNullOrWhiteSpace(secret.Identifier))
                throw new ArgumentException($"The value '{secret.Identifier ?? "(null)"}' provided as identifiers is not valid");

            var result = Environment.GetEnvironmentVariable(secret.Identifier);

            if (result != null)
                return await Task.FromResult(result.Trim());

            if (secret.Optional)
                return default;
            else
                throw new Exception($"No value found for Secret '{secret.Identifier}' on source '{secret.Source}'.");
        }
    }
}
