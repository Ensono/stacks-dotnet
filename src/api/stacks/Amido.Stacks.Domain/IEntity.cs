namespace Amido.Stacks.Domain
{
    public interface IEntity<TId>
    {
        TId Id { get; }
    }
}
