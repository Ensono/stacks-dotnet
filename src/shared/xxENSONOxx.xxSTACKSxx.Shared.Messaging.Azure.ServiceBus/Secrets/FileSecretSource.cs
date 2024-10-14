using System.IO;
using System.Threading.Tasks;
using xxENSONOxx.xxSTACKSxx.Shared.Messaging.Azure.ServiceBus.Exceptions;

namespace xxENSONOxx.xxSTACKSxx.Shared.Messaging.Azure.ServiceBus.Secrets;

public class FileSecretSource(string source) : ISecretSource<string>
{
    public string Source { get; } = source;

    public FileSecretSource() : this("FILE") { }

    public async Task<string> ResolveAsync(Secret secret)
    {
        if (secret == null)
            SecretNotDefinedException.Raise();

        if (string.IsNullOrWhiteSpace(secret!.Source))
            InvalidSecretDefinitionException.Raise(secret.Source, secret.Identifier);

        if (string.IsNullOrWhiteSpace(secret.Identifier))
            InvalidSecretDefinitionException.Raise(secret.Source, secret.Identifier);

        if (secret.Source!.ToUpperInvariant() != Source)
            SecretNotFoundException.Raise(secret.Source, secret.Identifier);

        if (!File.Exists(secret.Identifier))
        {
            if (!secret.Optional)
                SecretNotFoundException.Raise(secret.Source, secret.Identifier);

            return default;
        }

        using (var reader = File.OpenText(secret.Identifier))
        {
            var fileContents = await reader.ReadToEndAsync();
            return fileContents.Trim();
        }
    }
}
