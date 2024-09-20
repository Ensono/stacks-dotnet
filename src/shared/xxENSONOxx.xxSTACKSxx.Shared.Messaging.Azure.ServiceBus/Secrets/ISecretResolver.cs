using System.Threading.Tasks;

namespace xxENSONOxx.xxSTACKSxx.Shared.Messaging.Azure.ServiceBus.Secrets;

public interface ISecretResolver<T>
{
    void AddSource(ISecretSource<T> source);
    Task<T> ResolveSecretAsync(Secret secret);
}
