using System;
using System.IO;

namespace Amido.Stacks.Configuration
{
    public class FileSecretSource : ISecretSource<string>
    {
        public string Source { get; }

        public FileSecretSource() : this("FILE") { }

        public FileSecretSource(string source)
        {
            Source = source;
        }

        public string Resolve(Secret secret)
        {
            if (secret == null)
                throw new ArgumentNullException($"The parameter {nameof(secret)} cann't be null");

            if (secret.Source.ToUpperInvariant() != Source)
                throw new InvalidOperationException($"The source {secret.Source} does not match the source {Source}");

            if (string.IsNullOrWhiteSpace(secret.Identifier))
                throw new ArgumentException($"The value '{secret.Identifier ?? "(null)"}' provided as identifiers is not valid");

            if (!File.Exists(secret.Identifier))
            {
                if (secret.Optional)
                    return null;
                else
                    throw new Exception($"No value found for Secret '{secret.Identifier}' on source '{secret.Source}'.");
            }

            return File.ReadAllText(secret.Identifier).Trim();
        }
    }
}
