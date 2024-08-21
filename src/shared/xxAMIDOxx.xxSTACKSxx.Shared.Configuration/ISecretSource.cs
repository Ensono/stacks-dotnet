using System.Threading.Tasks;

namespace xxAMIDOxx.xxSTACKSxx.Shared.Configuration
{
    public interface ISecretSource<T>
    {
        string Source { get; }

        Task<T> ResolveAsync(Secret secret);
    }
}
