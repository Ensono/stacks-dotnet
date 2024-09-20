using Microsoft.Extensions.DependencyInjection;
using xxENSONOxx.xxSTACKSxx.BackgroundWorker.Secrets;

namespace xxENSONOxx.xxSTACKSxx.BackgroundWorker.Extensions;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Add the Secret resolver singleton with default secret sources (file and environment)
    /// </summary>
    public static IServiceCollection AddSecrets(this IServiceCollection services)
    {
        services.AddSingleton<ISecretResolver<string>, SecretResolver>();
        return services;
    }
}
