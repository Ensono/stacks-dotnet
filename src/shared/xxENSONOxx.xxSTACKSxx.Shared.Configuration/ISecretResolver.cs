using System.Threading.Tasks;

namespace xxENSONOxx.xxSTACKSxx.Shared.Configuration
{
    public interface ISecretResolver<T>
    {
        void AddSource(ISecretSource<T> source);
        Task<T> ResolveSecretAsync(Secret secret);
    }
}
