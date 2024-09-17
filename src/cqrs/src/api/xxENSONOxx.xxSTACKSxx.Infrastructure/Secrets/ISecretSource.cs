using System.Threading.Tasks;

namespace xxENSONOxx.xxSTACKSxx.Infrastructure.Secrets;

public interface ISecretSource<T>
{
    string Source { get; }

    Task<T> ResolveAsync(Secret secret);
}
