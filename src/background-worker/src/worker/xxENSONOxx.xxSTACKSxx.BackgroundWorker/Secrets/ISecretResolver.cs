using System.Threading.Tasks;

namespace xxENSONOxx.xxSTACKSxx.BackgroundWorker.Secrets;

public interface ISecretResolver<T>
{
    void AddSource(ISecretSource<T> source);
    Task<T> ResolveSecretAsync(Secret secret);
}
