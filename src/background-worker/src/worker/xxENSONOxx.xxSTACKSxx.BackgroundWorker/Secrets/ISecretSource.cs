using System.Threading.Tasks;

namespace xxENSONOxx.xxSTACKSxx.BackgroundWorker.Secrets;

public interface ISecretSource<T>
{
    string Source { get; }

    Task<T> ResolveAsync(Secret secret);
}
