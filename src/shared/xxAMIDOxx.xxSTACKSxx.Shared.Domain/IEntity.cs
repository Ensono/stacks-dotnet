namespace Amido.Stacks.Domain
{
    public interface IEntity<TIdentity>
    {
        TIdentity Id { get; }
    }
}
