using System.Collections.Generic;
using System.Threading.Tasks;
using xxENSONOxx.xxSTACKSxx.BackgroundWorker.Exceptions;

namespace xxENSONOxx.xxSTACKSxx.BackgroundWorker.Secrets;

public class SecretResolver : ISecretResolver<string>
{
    readonly Dictionary<string, ISecretSource<string>> sources = new();

    public SecretResolver()
    {
        AddSource(new EnvironmentSecretSource());
        AddSource(new FileSecretSource());
    }

    public void AddSource(ISecretSource<string> source)
    {
        sources.Add(source.Source.ToUpperInvariant(), source);
    }

    public async Task<string> ResolveSecretAsync(Secret secret)
    {
        if (secret == null)
            SecretNotDefinedException.Raise();

        if (string.IsNullOrWhiteSpace(secret!.Source))
            InvalidSecretDefinitionException.Raise(secret.Source, secret.Identifier);

        if (string.IsNullOrWhiteSpace(secret.Identifier))
            InvalidSecretDefinitionException.Raise(secret.Source, secret.Identifier);

        if (!sources.ContainsKey(secret.Source!.ToUpperInvariant()))
            InvalidSecretSourceException.Raise(secret.Source, secret.Identifier);

        var source = sources[secret.Source.ToUpperInvariant()];
        var result = await source.ResolveAsync(secret);

        if (result != null)
            return result;

        if (!secret.Optional)
            SecretNotFoundException.Raise(secret.Source, secret.Identifier);

        return default;
    }
}
