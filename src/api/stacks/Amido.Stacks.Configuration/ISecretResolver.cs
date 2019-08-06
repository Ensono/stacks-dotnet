namespace Amido.Stacks.Configuration
{
    public interface ISecretResolver<T>
    {
        void AddSource(ISecretSource<T> source);
        T ResolveSecret(Secret secret);
    }
}
