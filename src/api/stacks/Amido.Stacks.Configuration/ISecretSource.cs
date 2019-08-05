namespace Amido.Stacks.Configuration
{
    public interface ISecretSource
    {
        string Source { get; }

        string Resolve(Secret secret);
    }
}
