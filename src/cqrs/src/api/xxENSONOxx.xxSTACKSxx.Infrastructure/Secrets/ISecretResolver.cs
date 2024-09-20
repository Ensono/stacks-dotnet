using System.Threading.Tasks;

namespace xxENSONOxx.xxSTACKSxx.Infrastructure.Secrets;

public interface ISecretResolver<T>
{
    void AddSource(ISecretSource<T> source);
    Task<T> ResolveSecretAsync(Secret secret);
}
