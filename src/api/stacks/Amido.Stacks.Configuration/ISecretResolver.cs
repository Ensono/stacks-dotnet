using System.Threading.Tasks;

namespace Amido.Stacks.Configuration
{
    public interface ISecretResolver<T>
    {
        void AddSource(ISecretSource<T> source);
        Task<T> ResolveSecretAsync(Secret secret);
    }
}
