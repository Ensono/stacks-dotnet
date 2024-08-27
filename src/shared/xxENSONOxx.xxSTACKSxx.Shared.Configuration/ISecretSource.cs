using System.Threading.Tasks;

namespace xxENSONOxx.xxSTACKSxx.Shared.Configuration
{
    public interface ISecretSource<T>
    {
        string Source { get; }

        Task<T> ResolveAsync(Secret secret);
    }
}
