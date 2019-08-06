namespace Amido.Stacks.Configuration
{
    public interface ISecretSource<T>
    {
        string Source { get; }

        T Resolve(Secret secret);
    }
}
