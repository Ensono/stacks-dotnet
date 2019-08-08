using System.Threading.Tasks;

namespace Amido.Stacks.Configuration
{
    public interface ISecretSource<T>
    {
        string Source { get; }

        Task<T> ResolveAsync(Secret secret);
    }
}
