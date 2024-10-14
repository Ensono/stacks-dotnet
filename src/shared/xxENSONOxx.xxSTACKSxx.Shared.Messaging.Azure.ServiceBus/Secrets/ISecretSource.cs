using System.Threading.Tasks;

namespace xxENSONOxx.xxSTACKSxx.Shared.Messaging.Azure.ServiceBus.Secrets;

public interface ISecretSource<T>
{
    string Source { get; }

    Task<T> ResolveAsync(Secret secret);
}
