namespace Amido.Stacks.Configuration
{
    public interface ISecretResolver<T>
    {
        void AddSource(ISecretSource source);
        T ResolveSecret(Secret secret);
    }
}
