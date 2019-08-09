using System.IO;
using Amido.Stacks.Configuration.Exceptions;

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
                SecretNotDefinedException.Raise();

            if (string.IsNullOrWhiteSpace(secret.Source))
                InvalidSecretDefinitionException.Raise(secret.Source, secret.Identifier);

            if (string.IsNullOrWhiteSpace(secret.Identifier))
                InvalidSecretDefinitionException.Raise(secret.Source, secret.Identifier);

            if (secret.Source.ToUpperInvariant() != Source)
                SecretNotFoundException.Raise(secret.Source, secret.Identifier);

            if (!File.Exists(secret.Identifier))
            {
                if (!secret.Optional)
                    SecretNotFoundException.Raise(secret.Source, secret.Identifier);

                return default;
            }

            return File.ReadAllText(secret.Identifier).Trim();
        }
    }
}
